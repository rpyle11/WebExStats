namespace Stats.Api.Services;

public interface IWebExApiCalls
{
    Task<HttpResponseMessage> GetWebExData(string url, string appUser);
}