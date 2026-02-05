using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebExStatsSvc.Models;

namespace WebExStatsSvc.Services
{
    public class AppLogSvc
    {
        public static async Task<bool> LogAlert(AppLog appLog)
        {
            var log = new AppLogDto
            {
                AppName  = "WebExStatsSvc",
                AppUser = ServiceAcctName.GetServiceAccountName(),
                AppVersion = typeof(AppLogSvc).Assembly.GetName().Version.ToString(),
                EmailSubject = Properties.Settings.Default.AppLogEmailSubject,
                FromAddress = Properties.Settings.Default.FromEmail,
                LogDate = DateTime.Now,
                LogMessage = appLog.LogMsg,
                MessageType = appLog.MessageType.ToString(),
                SendEmailAddressList = appLog.SendEmail ? Properties.Settings.Default.NotifyEmail : string.Empty,
            };
            return await SendLog(log);
        }

        private static async Task<bool> SendLog(AppLogDto log)
        {

            var httpHandler = new HttpClientHandler
            {
                Credentials = CredentialCache.DefaultNetworkCredentials
            };
            using (var client = new HttpClient(httpHandler))
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.LogUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var response =  await client.PostAsJsonAsync(string.Empty, log);

                return response.IsSuccessStatusCode;


            }
        }
    }
}
