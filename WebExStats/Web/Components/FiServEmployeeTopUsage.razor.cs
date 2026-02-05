using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Stats.Models;
using Stats.Web.Services;
using System.Reflection;
using Microsoft.Extensions.Options;
using Stats.Web.Models;
using Telerik.Blazor.Components;

namespace Stats.Web.Components
{
    public partial class FiServEmployeeTopUsage
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        [Inject] private IApiDataService ApiDataService { get; set; }

        [Inject] private ILogService LogService { get; set; }

        [Inject] private IOptions<AppSettings> Settings { get; set; }

        private List<TopUsersDto> ChartData { get; set; } = new();

        private EmployeeMeetingDetails DetailsWindow { get; set; }

        private DateTime StartDate { get; set; }

        private DateTime EndDate { get; set; }

        private DateTime MinDate { get; set; }
        private DateTime MaxDate { get; set; }
        public string AppUser { get; set; }

        private bool PageWait { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            AppUser = authState.User.Identity?.Name?.Split('\\').Last();

            await LoadInitialDates();
            SetInitialDates();

            await LoadData();
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

                StateHasChanged();

            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppErrorLogSetup(AppUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;



            }
        }
        private void SetInitialDates()
        {
            StartDate = MaxDate.AddMonths(-12);
            EndDate = MaxDate;
        }

        private async void StartDateChanged(DateTime date)
        {
            try
            {
                StartDate = date;
                await LoadData();
            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppErrorLogSetup(AppUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;



            }
        }

        private async void EndDateChanged(DateTime date)
        {
            try
            {
                EndDate = date;
                await LoadData();
            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppErrorLogSetup(AppUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;
            }
        }

        private async Task LoadData()
        {
            try
            {
                PageWait = true;
                StateHasChanged();

                var data = await ApiDataService.GetTopFiServUsers(new EmployeeMeetingParametersFilter
                {
                    StartDate = StartDate.Date,
                    EndDate = EndDate.Date,
                    Filters = Settings.Value.MonthlyTotalFilters
                }, AppUser);

                if (data != null)
                {
                    ChartData = data;
                }



                PageWait = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppErrorLogSetup(AppUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;
            }
        }

        private void ChartClicked(ChartSeriesClickEventArgs args)
        {
            var item = (TopUsersDto)args.DataItem;

            DetailsWindow.DisplayName = item.DisplayName;
            DetailsWindow.Email = item.Email;
            DetailsWindow.StartDate = StartDate;
            DetailsWindow.EndDate = EndDate;
            DetailsWindow.EmployeeTotalTime = 0;
            DetailsWindow.DialogTitle = $"Meeting Details for {item.DisplayName}";

            DetailsWindow.ShowDialog();

        }

       
    }
}
