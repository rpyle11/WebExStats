using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Stats.Models;
using Stats.Web.Services;

namespace Stats.Web.Components
{
    public partial class EmployeeMeetings
    {
        [Parameter]
        public string EmployeeEmail { get; set; }

        [Parameter]
        public string DisplayName { get; set; }

        [Parameter]
        public int TotalEmployeeTime { get; set; }

        [Parameter]
        public DateTime StartDate { get; set; }

        [Parameter]
        public DateTime EndDate { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        [Inject] private IApiDataService ApiDataService { get; set; }

        [Inject] private ILogService LogService { get; set; }

        private List<EmployeeMeetingDto> Meetings { get; set; }

        private List<CompareTimeDto> CompareMeetings { get; set; }

        public string AppUser { get; set; }

        private bool BarChartWait { get; set; }

        private bool PieChartWait { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                BarChartWait = true;
                PieChartWait = true;
                var authState = await AuthenticationStateTask;
                AppUser = authState.User.Identity?.Name?.Split('\\').Last();

                if (TotalEmployeeTime == 0)
                {
                    var empTime = await ApiDataService.GetEmployeeMeetingTotalTime(new EmployeeTotalTimeParameters
                    {
                        DisplayName = DisplayName,
                        Email = EmployeeEmail,
                        EndDate = EndDate,
                        StartDate = StartDate
                    }, AppUser);

                    if (empTime != null)
                    {
                        TotalEmployeeTime = empTime.EmployeeTotalTime;
                    }
                }

                var parameters = new EmployeeMeetingsListParameters
                {
                    Email = EmployeeEmail,
                    DisplayName = DisplayName,
                    TotalEmpTime = TotalEmployeeTime,
                    StartDate = StartDate,
                    EndDate = EndDate
                };

                var barData = await ApiDataService.GetEmployeeMeetings(parameters, AppUser);

                if (barData != null)
                {
                    Meetings = barData.OrderByDescending(o => o.PercentOfTotalTime).ToList();
                }

                var compareData = await ApiDataService.GetEmployeeMeetingsTimeCompare(parameters, AppUser);
                if (compareData != null)
                {
                    CompareMeetings = compareData;
                }

                BarChartWait = false;
                PieChartWait = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await LogService.LogAlert(AppLogPrep.AppErrorLogSetup(AppUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }
           
        }
    }
}
