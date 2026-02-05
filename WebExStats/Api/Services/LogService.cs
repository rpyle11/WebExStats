using Stats.Api.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Stats.Models;

namespace Stats.Api.Services
{
    public class LogService : ILogService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _httpClient;

        public LogService(IOptions<AppSettings> settings, HttpClient httpClient)
        {
            _settings = settings;
            _httpClient = httpClient;
        }

        public async Task<bool> LogAlert(AppLog appLog)
        {
            var log = new AppLogDto
            {
                AppName = "Stats.Api",
                AppUser = appLog.AppUser,
                AppVersion = typeof(LogService).Assembly.GetName().Version?.ToString(),
                EmailSubject = _settings.Value.AppLogEmailSubject,
                FromAddress = _settings.Value.AppLogFromEmail,
                LogDate = DateTime.Now,
                LogMessage = appLog.LogMsg,
                MessageType = appLog.MessageType.ToString(),
                SendEmailAddressList = appLog.SendEmail ? _settings.Value.AppLogNotifyEmail : string.Empty,
            };
            return await SendLog(log);
        }

        private async Task<bool> SendLog(AppLogDto log)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.PostAsJsonAsync(string.Empty, log);

            return response.IsSuccessStatusCode;
        }

    }
}
