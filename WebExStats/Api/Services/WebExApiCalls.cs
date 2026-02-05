using Stats.Api.Entities;
using Stats.Api.Models;
using Stats.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

namespace Stats.Api.Services
{
    public class WebExApiCalls : IWebExApiCalls
    {
       
        private readonly ILogService _logService;
        private readonly IRepositoryService _repositoryService;
        private readonly HttpClient _httpClient;
        public WebExApiCalls(HttpClient httpClient, IRepositoryService repositoryService, ILogService logService)
        {
            _httpClient = httpClient;
            _repositoryService = repositoryService;
            _logService = logService;
        }

        public async Task<HttpResponseMessage> GetWebExData(string url,  string appUser)
        {
            try
            {
                var tokenList = await _repositoryService.GetWebExApiTokens(appUser);

                var accessToken = tokenList.FirstOrDefault(f => f.TokenName == "access")?.Token;
                if (accessToken == null)
                {
                    throw new InvalidDataException("Unable to read access token from token list");
                }

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(accessToken, "Bearer");

                var response = await _httpClient.GetAsync(new Uri(url));

                if (response.StatusCode == HttpStatusCode.Unauthorized)

                {
                    var newToken = await RefreshToken(tokenList, appUser);

                    if (newToken == null)
                    {
                        throw new WebException("Unable to get new access token");
                    }

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(newToken.TokenType, newToken.AccessToken);

                    response = await _httpClient.GetAsync(new Uri(url));
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }


        private async Task<AuthToken> RefreshToken(IReadOnlyCollection<Tokens> tokensList, string appUser)
        {
            try
            {
                var form = new Dictionary<string, string>
                {
                    {"grant_type", "refresh_token"},
                    {"client_id", tokensList.FirstOrDefault(f => f.TokenName=="clientId")?.Token},
                    {"client_secret",tokensList.FirstOrDefault(f => f.TokenName=="clientsecret")?.Token},
                    {"refresh_token", tokensList.FirstOrDefault(f => f.TokenName=="refresh")?.Token}

                };

                _httpClient.DefaultRequestHeaders.Clear();

                var response =
                    await _httpClient.PostAsync($"{_httpClient.BaseAddress}/access_token", new FormUrlEncodedContent(form));

                if (response.IsSuccessStatusCode)
                {
                    var updatedToken = await response.Content.ReadAsAsync<AuthToken>();
                    await _repositoryService.UpdateWebExApiToken(new Tokens
                    {
                        Token = updatedToken.AccessToken,
                        TokenName = "access",
                        Id = tokensList.FirstOrDefault(f => f.TokenName == "access")!.Id
                    }, appUser);

                    return updatedToken;
                }

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }
    }
}
