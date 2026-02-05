using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Stats.Models;
using Stats.Web.Services;
using System.Reflection;


namespace Stats.Web.Pages.Reports;

public partial class AllEmployeesAllMeetings
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    [Inject] private IApiDataService ApiDataService { get; set; }

    [Inject] private ILogService LogService { get; set; }

    [Inject] private NavigationManager NavManager { get; set; }

    private List<EmployeesAllMeetingsDto> GridData { get; set; } = new();

    private DateTime StartDate { get; set; }=DateTime.Now;

    private DateTime EndDate { get; set; }=DateTime.Now;

    private DateTime MinDate { get; set; }
    private DateTime MaxDate { get; set; }

    private string AppUser { get; set; }

    private bool PageWait { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        AppUser = authState.User.Identity?.Name?.Split('\\').Last();

        await LoadInitialDates();
       
        EndDate = MaxDate;
        StartDate = EndDate.AddMonths(-12);

        await LoadGridData();

        StateHasChanged();
       
    }

    private async Task LoadInitialDates()
    {
        try
        {
           
            var data = await ApiDataService.GetStatisticDateRange(AppUser);

            if (data == null) return;
            MinDate = data.StartDate.AddDays(-1);
            MaxDate = data.EndDate;

          

        }
        catch (Exception ex)
        {
            await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager.Uri,
                MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

            NavManager.NavigateTo("./error");

        }
    }

    private async void StartDateChanged(DateTime dtDate)
    {
        StartDate = dtDate;
        await LoadGridData();
        
    }

    private async void EndDateChanged(DateTime dtDate)
    {
        EndDate = dtDate;
        await LoadGridData();
    }

    private async Task LoadGridData()
    {
        try
        {
            PageWait = true;
            StateHasChanged();
            var parameters = new EmployeeMeetingParameters
            {
                EndDate = EndDate,
                StartDate = StartDate
            };


            var data = await ApiDataService.GetEmployeesAllMeetings(parameters, AppUser);

            if (data != null)
            {
                GridData = data.OrderBy(o => o.Title).ThenBy(th => th.DisplayName).ToList();
            }


            PageWait=false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager.Uri,
                MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

            NavManager.NavigateTo("./error");

        }
    }

    


}