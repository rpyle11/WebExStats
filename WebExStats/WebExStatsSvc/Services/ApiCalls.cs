using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using WebExStatsSvc.Models;

namespace WebExStatsSvc.Services
{
    public static class ApiCalls
    {
        public static async Task<bool> GetMeetingsCall(MeetingParameters parameters)
        {
            try
            {
                var httpHandler = new HttpClientHandler
                {
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };

                using (var client = new HttpClient(httpHandler))
                {

                    client.BaseAddress = new Uri(Properties.Settings.Default.WebExStatsMeetingUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("appuser", ServiceAcctName.GetServiceAccountName());
                    client.Timeout = TimeSpan.FromMinutes(30);

                    var response = await client.PostAsJsonAsync(string.Empty, parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new WebException($"Server response from url {Properties.Settings.Default.WebExStatsMeetingUrl} was 500 status code");
                    }

                }
            }
            catch (Exception ex)
            {
                await AppLogSvc.LogAlert(new AppLog
                {
                    LogMsg =
                        $"{ex.GetType().Name} Error in {MethodName.GetMethodName(MethodBase.GetCurrentMethod())} Error: {ex.Message}",
                    MessageType = AppLog.MessageTypeEnum.Error,
                    SendEmail = true
                });
            }
           

            return false;

        }

        public static async Task<bool> GetMeetingParticipantsCall()
        {
            try
            {
                var httpHandler = new HttpClientHandler
                {
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };

                using (var client = new HttpClient(httpHandler))
                {
                    client.BaseAddress = new Uri(Properties.Settings.Default.WebExStatusParticipantsUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("appuser", ServiceAcctName.GetServiceAccountName());
                    client.Timeout = TimeSpan.FromMinutes(30);

                    var response = await client.GetAsync(string.Empty);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new WebException($"Server response from url {Properties.Settings.Default.WebExStatusParticipantsUrl} was 500 status code");
                    }

                }
            }
            catch (Exception ex)
            {
                await AppLogSvc.LogAlert(new AppLog
                {
                    LogMsg =
                        $"{ex.GetType().Name} Error in {MethodName.GetMethodName(MethodBase.GetCurrentMethod())} Error: {ex.Message}",
                    MessageType = AppLog.MessageTypeEnum.Error,
                    SendEmail = true
                });
            }
           

            return false;

        }

        public static async Task<LastDataPullDto> GetLastPull()
        {
            try
            {
                var httpHandler = new HttpClientHandler
                {
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };
                using (var client = new HttpClient(httpHandler))
                {
                    client.BaseAddress = new Uri(Properties.Settings.Default.WebExStatsLastPullUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("appuser", ServiceAcctName.GetServiceAccountName());
                    client.Timeout = TimeSpan.FromMinutes(5);

                    var response = await client.GetAsync(string.Empty);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<LastDataPullDto>();
                    }

                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new WebException($"Server response from url {Properties.Settings.Default.WebExStatsLastPullUrl} was 500 status code");
                    }

                }
            }
            catch (Exception ex)
            {
                await AppLogSvc.LogAlert(new AppLog
                {
                    LogMsg =
                        $"{ex.GetType().Name} Error in {MethodName.GetMethodName(MethodBase.GetCurrentMethod())} Error: {ex.Message}",
                    MessageType = AppLog.MessageTypeEnum.Error,
                    SendEmail = true
                });
            }
            

            return null;
        }


    }
}
