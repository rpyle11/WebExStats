using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Stats.Models;
using Stats.Web.Services;

namespace Stats.Web.Components;

public partial class MeetingParticipants
{
    [Parameter]
    public string MeetingId { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    [Inject] private IApiDataService ApiDataService { get; set; }

    [Inject] private ILogService LogService { get; set; }

    private List<MeetingParticipantDto> Participants { get; set; }

    public string AppUser { get; set; }

    private bool PageWait { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            PageWait = true;
            var authState = await AuthenticationStateTask;
            AppUser = authState.User.Identity?.Name?.Split('\\').Last();

            var data = await ApiDataService.GetMeetingParticipants(
                new MeetingEmployeesParameters { MeetingId = MeetingId }, AppUser);

            if (data != null)
            {
                Participants = data.OrderByDescending(o => o.ParticipantTime).ToList();
            }

            PageWait = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await LogService.LogAlert(AppLogPrep.AppErrorLogSetup(AppUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
        }
    }
}