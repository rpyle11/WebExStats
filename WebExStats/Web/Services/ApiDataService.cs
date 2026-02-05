using System.Net;
using Stats.Models;
using System.Reflection;

namespace Stats.Web.Services
{
    public class ApiDataService : IApiDataService
    {
        private readonly ILogService _logService;
      
        private readonly HttpClient _httpClient;

        public ApiDataService(HttpClient httpClient, ILogService logService)
        {
            _logService=logService;
            _httpClient=httpClient; 
        }

        public async Task<StartEndDatesDto> GetStatisticDateRange(string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/dataui/startenddates");


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<StartEndDatesDto>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<EmployeesAllMeetingsDto>> GetEmployeesAllMeetings(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/EmployeesInAllMeetingsByDate", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<EmployeesAllMeetingsDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TotalEmployeeTimeInAllMeetingsDto>> GetEmployeeTotalTimeInMeetings(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/TotalEmployeeTimeAllMeetingsByDate", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<TotalEmployeeTimeInAllMeetingsDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<ByEmployeeTotalTimeInMeetingsDto>> GetByEmployeeTotalTimeInMeetings(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/ByEmployeeTotalTimeByValues", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<ByEmployeeTotalTimeInMeetingsDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TopUsersDto>> GetTopUsers(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/TopUsers", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<TopUsersDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TopUsersDto>> GetTopFiServUsers(EmployeeMeetingParametersFilter parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/TopFiServUsers", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<TopUsersDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<MonthTotalSeriesDto>> GetMonthlyTotals(MonthlyTotalsParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/MonthlyTotals", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<MonthTotalSeriesDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<MeetingParticipantDto>> GetMeetingParticipants(MeetingEmployeesParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/dataui/meetingpaticipants/{parameters.MeetingId}");


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<MeetingParticipantDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<EmployeeMeetingDto>> GetEmployeeMeetings(EmployeeMeetingsListParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/EmployeeMeetings", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<EmployeeMeetingDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<CompareTimeDto>> GetEmployeeMeetingsTimeCompare(EmployeeMeetingsListParameters parameters, string appUser)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/EmployeeMeetingsCompare", parameters);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<List<CompareTimeDto>>();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<EmployeeMeetingTotalTimeDto> GetEmployeeMeetingTotalTime(
            EmployeeTotalTimeParameters parameters, string appUser)
        {
            try
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Add("appuser", appUser);

                    var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/dataui/EmployeeTotalTime", parameters);


                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return await response.Content.ReadAsAsync<EmployeeMeetingTotalTimeDto>();
                    }
                }
                catch (Exception ex)
                {
                    await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
