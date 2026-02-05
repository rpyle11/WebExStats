using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Stats.Models;
using Stats.Web.Services;
using System.Reflection;

namespace Stats.Web.Pages.Reports;

public partial class ByEmployeeMeetingTotalTime
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    [Inject] private IApiDataService ApiDataService { get; set; }

    [Inject] private ILogService LogService { get; set; }

    [Inject] private NavigationManager NavManager { get; set; }

    private List<ByEmployeeTotalTimeInMeetingsDto> GridData { get; set; } = new();

    private DateTime StartDate { get; set; } = DateTime.Now;

    private DateTime EndDate { get; set; }=DateTime.Now;

    private DateTime MinDate { get; set; }
   
    private DateTime MaxDate { get; set; }

    private string AppUser { get; set; }

    private bool PageWait { get; set; }

    private bool OnlyGsb { get; set; }

    private bool OnlyFiServ { get; set; }

    private bool OnlyCatalyst { get; set; }

    private bool RemovePhones { get; set; }

    private string SelectedGsbBtnColor { get; set; } = "secondary";

    private string SelectedFiServBtnColor { get; set; } = "secondary";

    private string SelectedNoPhonesBtnColor { get; set; } = "secondary";

    private string SelectedCatalystBtnColor { get; set; } = "secondary";

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        AppUser = authState.User.Identity?.Name?.Split('\\').Last();

        await LoadInitialDates();

        EndDate = MaxDate;
        StartDate = EndDate.AddMonths(-12);

        OnlyGsb = true;
        OnlyFiServ = true;
        OnlyCatalyst = true;

        SelectedGsbBtnColor = OnlyGsb ? "primary" : "secondary";
        SelectedFiServBtnColor = OnlyFiServ ? "primary" : "secondary";
        SelectedCatalystBtnColor = OnlyCatalyst ? "primary" : "secondary";

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

            StateHasChanged();

        }
        catch (Exception ex)
        {
            await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager.Uri,
                MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

            NavManager.NavigateTo("./error");

        }
    }

    private async void GsbOnlyToggle()
    {
        OnlyGsb = !OnlyGsb;
       
        SelectedGsbBtnColor = OnlyGsb ? "primary" : "secondary";

        await LoadGridData();

        StateHasChanged();

    }

    private async void FiServOnlyToggle()
    {
        OnlyFiServ = !OnlyFiServ;

        SelectedFiServBtnColor = OnlyFiServ ? "primary" : "secondary";

        await LoadGridData();

        StateHasChanged();
        
    }

    private async void RemovePhonesToggle()
    {
        RemovePhones = !RemovePhones;
       
        SelectedNoPhonesBtnColor = RemovePhones ? "primary" : "secondary";

        await LoadGridData();

        StateHasChanged();
       
       
    }

    private async void CatalystOnlyToggle()
    {
        OnlyCatalyst = !OnlyCatalyst;
        
        SelectedCatalystBtnColor = OnlyCatalyst ? "primary" : "secondary";

        await LoadGridData();

        StateHasChanged();
       
       
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

         
            var parameters = new EmployeeMeetingParameters
            {
                EndDate = EndDate,
                StartDate = StartDate
            };
           

            var data = await ApiDataService.GetByEmployeeTotalTimeInMeetings(parameters, AppUser);

            if (data != null)
            {
              
                GridData = await ApplyDataFilter(data);
                StateHasChanged();
              
              
            }

            PageWait = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager.Uri,
                MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

            NavManager.NavigateTo("./error");

        }
    }

    private async Task<List<ByEmployeeTotalTimeInMeetingsDto>> ApplyDataFilter(List<ByEmployeeTotalTimeInMeetingsDto> data)
    {
        try
        {
            const string catalyst = "catalyst";
            const string gsbEmail = "greatsouthernbank.com";
            const string fiServEmail = "fiserv.com";
            const string noPhone = "*";

            var returnData = new List<ByEmployeeTotalTimeInMeetingsDto>();

            switch (OnlyCatalyst)
            {
                case false when !OnlyGsb && !OnlyFiServ && !RemovePhones:
                    returnData = data;
                    break;
                case true when OnlyGsb && OnlyFiServ && RemovePhones:
                {
                    var c = data.Where(w =>
                        w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));

                    var gf = data.Where(w =>
                        (w.Email.ToLower().Contains(gsbEmail) || w.Email.ToLower().Contains(fiServEmail)) && !w.DisplayName.ToLower().Contains(catalyst));

                    returnData.AddRange(c);
                    returnData.AddRange(gf);
                
                    returnData = returnData.ToList();
                    break;
                }
            }

            switch (OnlyCatalyst)
            {
                case true when !OnlyGsb && !OnlyFiServ && !RemovePhones:
                    returnData.AddRange(data.Where(w =>
                        w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail)));
                    break;
                case false when OnlyGsb && !OnlyFiServ && !RemovePhones:
                    returnData.AddRange(data.Where(w =>
                        w.Email.ToLower().Contains(gsbEmail) && !w.DisplayName.ToLower().Contains(catalyst)));
                    break;
            }

            if (!OnlyCatalyst && !OnlyGsb && OnlyFiServ && !RemovePhones)
            {
                returnData.AddRange(data.Where(w => w.Email.ToLower().Contains(fiServEmail)));
            }

            switch (OnlyCatalyst)
            {
                case false when !OnlyGsb && !OnlyFiServ && RemovePhones:
                    returnData.AddRange(data.Where(w => !w.DisplayName.ToLower().Contains(noPhone)));
                    break;
                case true when OnlyGsb && !OnlyFiServ && !RemovePhones:
                {
                    var c = data.Where(w =>
                        w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));

                    var g = data.Where(w =>
                        !w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));

                    returnData.AddRange(c);
                    returnData.AddRange(g);
                    break;
                }
            }

            if (OnlyCatalyst && !OnlyGsb && OnlyFiServ && !RemovePhones)
            {
                var c = data.Where(w =>
                    w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));

                var f = data.Where(w => w.Email.ToLower().Contains(fiServEmail));

                returnData.AddRange(c);
                returnData.AddRange(f);

            }

            switch (OnlyCatalyst)
            {
                case true when !OnlyGsb && !OnlyFiServ && RemovePhones:
                {
                    var c = data.Where(w =>
                        w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));

               
                    returnData.AddRange(c.Where(w => !w.DisplayName.ToLower().Contains(noPhone)));
                    break;
                }
                case false when OnlyGsb && !OnlyFiServ && RemovePhones:
                {
                    var g = data.Where(w =>
                        !w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));

               
                    returnData.AddRange(g.Where(w => !w.DisplayName.ToLower().Contains(noPhone)));
                    break;
                }
            }

            if (!OnlyCatalyst && OnlyGsb && OnlyFiServ && !RemovePhones)
            {

                var g = data.Where(w =>
                    !w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));

                var f = data.Where(w => w.Email.ToLower().Contains(fiServEmail));

                returnData.AddRange(g);
                returnData.AddRange(f);

            }

            switch (OnlyCatalyst)
            {
                case false when !OnlyGsb && OnlyFiServ && RemovePhones:
                {
                    var f = data.Where(w => w.Email.ToLower().Contains(fiServEmail));

               
                    returnData.AddRange(f.Where(w => !w.DisplayName.ToLower().Contains(noPhone)));
                    break;
                }
                case true when OnlyGsb && OnlyFiServ && !RemovePhones:
                {
                    var c = data.Where(w =>
                        w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));
                    var g = data.Where(w =>
                        !w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));
                    var f = data.Where(w => w.Email.ToLower().Contains(fiServEmail));

               
                    returnData.AddRange(c);
                    returnData.AddRange(g);
                    returnData.AddRange(f);
                    break;
                }
            }

            switch (OnlyCatalyst)
            {
                case false when OnlyGsb && OnlyFiServ && RemovePhones:
                {
                    var g = data.Where(w =>
                        !w.DisplayName.ToLower().Contains(catalyst) && !w.DisplayName.ToLower().Contains(noPhone) && w.Email.ToLower().Contains(gsbEmail));
                    var f = data.Where(w => w.Email.ToLower().Contains(fiServEmail));
               
                    returnData.AddRange(g);
                    returnData.AddRange(f);
                    break;
                }
                case true when OnlyGsb && !OnlyFiServ && RemovePhones:
                {
                    var c = data.Where(w =>
                        w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));
                    var g = data.Where(w =>
                        !w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));
              
               
                    returnData.AddRange(c);
                    returnData.AddRange(g.Where(w => !w.DisplayName.ToLower().Contains(noPhone)));
                    break;
                }
            }

            if (!OnlyCatalyst || OnlyGsb || !OnlyFiServ || !RemovePhones)
                return returnData.OrderByDescending(o => o.EmployeeTotalTime).ToList();
            {
                var c = data.Where(w =>
                    w.DisplayName.ToLower().Contains(catalyst) && w.Email.ToLower().Contains(gsbEmail));
                var f = data.Where(w => w.Email.ToLower().Contains(fiServEmail));
              
               
                returnData.AddRange(c);
                returnData.AddRange(f.Where(w => !w.DisplayName.ToLower().Contains(noPhone)));
            }

            return returnData.OrderByDescending(o => o.EmployeeTotalTime).ToList();


        }
        catch (Exception ex)
        {
            await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager.Uri,
                MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

            NavManager.NavigateTo("./error");

        }

        return new List<ByEmployeeTotalTimeInMeetingsDto>();
    }

    

   

 

    
}