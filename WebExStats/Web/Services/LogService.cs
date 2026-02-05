using Stats.Models;

using Microsoft.Extensions.Options;
using Stats.Web.Models;


namespace Stats.Web.Services
{
    public class LogService : ILogService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _httpClient;

        public LogService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _settings = settings;
            _httpClient = httpClient;
        }

        public async Task<bool> LogAlert(AppLog appLog)
        {
            var log = new AppLogDto
            {
                AppName = "Stats.Web",
                AppUser = appLog.AppUser,
                AppVersion = typeof(LogService).Assembly.GetName().Version?.ToString(),
                EmailSubject = _settings.Value.LogEmailSubject,
                FromAddress = _settings.Value.LogFromAddress,
                LogDate = DateTime.Now,
                LogMessage = appLog.LogMsg,
                MessageType = appLog.MessageType.ToString(),
                SendEmailAddressList = appLog.SendEmail ? _settings.Value.LogToAddress : string.Empty,
            };
            return await SendLog(log);
        }

        private async Task<bool> SendLog(AppLogDto log)
        {

            _httpClient.DefaultRequestHeaders.Clear();
            var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress, log);
            return response.IsSuccessStatusCode;

        }

    }
}
