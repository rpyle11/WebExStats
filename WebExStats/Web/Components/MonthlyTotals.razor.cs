using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Stats.Models;
using Stats.Web.Services;
using System.Reflection;

namespace Stats.Web.Components
{
    public partial class MonthlyTotals
    {
        [Parameter] public List<string> MonthlyFilters { get; set; } = new();
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        [Inject] private IApiDataService ApiDataService { get; set; }

        [Inject] private ILogService LogService { get; set; }



        private List<MonthTotalSeriesDto> ChartData { get; set; } = new();

        private List<DtPickerData> DtPickerList { get; set; }

        private DtPickerData SelectedStartDate { get; set; } = new();
        private DtPickerData SelectedEndDate { get; set; } = new();

        private string[] DateAxisData { get; set; }



        private DateTime StartDate { get; set; }

        private DateTime EndDate { get; set; }

        private DateTime MinDate { get; set; }
        private DateTime MaxDate { get; set; }
        private string AppUser { get; set; }

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



        private void SetInitialDates()
        {
            StartDate = MinDate;
            EndDate = MaxDate;

            PopPickerDates();
        }

        private async Task LoadInitialDates()
        {
            try
            {

                var data = await ApiDataService.GetStatisticDateRange(AppUser);

                if (data == null) return;
                MinDate = data.StartDate;
                MaxDate = data.EndDate;

                StateHasChanged();

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
                var data = await ApiDataService.GetMonthlyTotals(new MonthlyTotalsParameters
                {
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Filters = MonthlyFilters

                }, AppUser);

                if (data != null)
                {


                    DateAxisData = data.Select(s => s.MonthDate).ToArray();

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

        private async void StartDateChanged(int id)
        {

            var val = DtPickerList.FirstOrDefault(f => f.Id == id);

            if (val == null) return;
            StartDate = val.FilterDate;
            SelectedStartDate = val;
            await LoadData();
        }

        private async void EndDateChanged(int id)
        {

            var val = DtPickerList.FirstOrDefault(f => f.Id == id);
            if (val == null) return;
            EndDate = val.FilterDate;
            SelectedEndDate = val;
            await LoadData();
        }


        private void PopPickerDates()
        {
            var lp = 1;
            DtPickerList = new List<DtPickerData>();

            var values = MonthsBetween(MinDate, MaxDate);

            foreach (var dtDate in values)
            {
                DtPickerList.Add(new DtPickerData
                {
                    Id = lp++,
                    MonthDate = $"{dtDate.Month}/{dtDate.Year}",
                    FilterDate = dtDate.DateValue

                });
            }

            SelectedStartDate = DtPickerList.FirstOrDefault();
            SelectedEndDate = DtPickerList.LastOrDefault();

            StateHasChanged();
        }

        private static IEnumerable<(int Month, int Year, DateTime DateValue)> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }


            while (iterator <= limit)
            {
                yield return (iterator.Month, iterator.Year, iterator);

                iterator = iterator.AddMonths(1);
            }
        }



    }


    public class DtPickerData
    {
        public int Id { get; set; }
        public string MonthDate { get; set; }

        public DateTime FilterDate { get; set; }
    }


}
