using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Stats.Models;
using Stats.Web.Models;
using Stats.Web.Services;

namespace Stats.Web.Components
{
    public partial class UserInfo
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        [Inject] private IOptions<AppSettings> Settings { get; set; }
        [Inject] ILogService LogService { get; set; }

        private string FullName { get; set; } = "Unknown";

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User.Identity?.Name?.Split('\\').Last();
            SetFullName(string.IsNullOrEmpty(user) ? "unknown" : user);

        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public async void SetFullName(string appUser)
        {
            using var pc = new PrincipalContext(ContextType.Domain);
            FullName = UserPrincipal.FindByIdentity(pc, appUser)?.DisplayName;

           
            await LogService?.LogAlert(new AppLog
            {
                AppUser = appUser,
                LogMsg = $"User {FullName} accessed WebEx Stats site",
                MessageType = AppLog.MessageTypeEnum.Message,
                SendEmail = false
            })!;
        }
    }
}
