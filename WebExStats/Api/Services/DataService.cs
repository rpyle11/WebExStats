using AutoMapper;
using Microsoft.Extensions.Options;
using Stats.Api.Entities;
using Stats.Api.Models;
using Stats.Api.Models.WebExMeeting;
using Stats.Models;
using System.Reflection;

namespace Stats.Api.Services
{
    public class DataService : IDataService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IWebExApiCalls _webExApiCalls;
        private readonly ILogService _logService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IMapper _mapper;

        public DataService(IRepositoryService repositoryService, ILogService logService, IOptions<AppSettings> settings, IWebExApiCalls webExApiCalls, IMapper mapper)
        {
            _repositoryService = repositoryService;
            _logService = logService;
            _settings = settings;
            _webExApiCalls = webExApiCalls;
            _mapper = mapper;
        }



        public async Task<bool> PullMeetings(MeetingParameters parameters, string appUser)
        {
            try
            {
                if (parameters.FromDate.HasValue && parameters.ToDate.HasValue)
                {

                    if (parameters.FromDate.Value.Date == parameters.ToDate.Value.Date)
                    {
                        var data = await GetWebExApiMeetings(parameters.FromDate.Value, appUser);
                        if (data)
                        {
                            return await _repositoryService.AddPullLog(new DataPullLogs
                            {
                                Parameters = $"From Date {parameters.FromDate.Value} To Date {parameters.ToDate.Value}",
                                PullDate = DateTime.Now,
                                PullType = "Meeting"
                            }, appUser);
                        }

                    }


                    foreach (var dt in EachCalendarDay(parameters.FromDate.Value, parameters.ToDate.Value))
                    {
                        if (!await GetWebExApiMeetings(dt, appUser))
                        {
                            return false;
                        }

                    }

                    return await _repositoryService.AddPullLog(new DataPullLogs
                    {
                        Parameters = $"From Date {parameters.FromDate.Value} To Date {parameters.ToDate.Value}",
                        PullDate = DateTime.Now,
                        PullType = "Meeting"
                    }, appUser);


                }

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;
        }

        public async Task<bool> PullMeetingParticipants(string appUser)
        {
            try
            {
                var meetings = await _repositoryService.GetMeetingsPullParticipants(appUser);

                if (meetings is not { Count: > 0 }) return true;
                foreach (var meeting in meetings)
                {
                    if (!await GetWebExApiMeetingParticipants(meeting.Id, meeting.HostEmail,
                            appUser))
                    {
                        throw new InvalidDataException(
                            $"Unable to add participants for meeting {meeting.Title}, meeting Id {meeting.Id}");
                    }

                    if (!await _repositoryService.UpdateMeetingParticipantsPulled(meeting.Id, appUser))
                    {
                        throw new InvalidDataException(
                            $"Unable to update participants pulled meeting Id {meeting.Id}");
                    }
                }

                return await _repositoryService.AddPullLog(new DataPullLogs
                {
                    Parameters = "Participants Pull",
                    PullDate = DateTime.Now,
                    PullType = "Participants"
                }, appUser);


            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;
        }

        public async Task<LastDataPullDto> GetLastDataPullByType(LastDataPullParameters parameters, string appUser)
        {
            try
            {
                var data = await _repositoryService.GetLastPullByType(parameters.PullType, appUser);
                if (data != null)
                {
                    return _mapper.Map<LastDataPullDto>(data);
                }

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<StartEndDatesDto> GetStartEndDates(string appUser)
        {
            try
            {
                var data = await _repositoryService.GetStartEndDates(appUser);

                if (data != null)
                {
                    return _mapper.Map<StartEndDatesDto>(data);
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<EmployeesAllMeetingsDto>> GetAllEmployeesInAllMeetings(
            EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                var data = await _repositoryService.GetEmployeesInAllMeetings(parameters, appUser);

                if (data != null)
                {
                    return _mapper.Map<List<EmployeesAllMeetingsDto>>(data);
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TotalEmployeeTimeInAllMeetingsDto>> GetEmployeeTotalTimeInAllMeetings(
            EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                var data = await _repositoryService.GetEmployeesTotalTimeInMeetings(parameters, appUser);

                if (data != null)
                {
                    return _mapper.Map<List<TotalEmployeeTimeInAllMeetingsDto>>(data);
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<ByEmployeeTotalTimeInMeetingsDto>> GetByEmployeeMeetingsTotalTime(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                var startDate = new DateTime(parameters.StartDate.Year, parameters.StartDate.Month,
                    parameters.StartDate.Day, 0, 0, 0);
                var endDate = new DateTime(parameters.EndDate.Year, parameters.EndDate.Month,
                    parameters.EndDate.Day, 23, 59, 59);
                var data = await _repositoryService.GetByEmployeesTotalMeetingsTime(startDate, endDate, appUser);

                if (data != null)
                {

                    return _mapper.Map<List<ByEmployeeTotalTimeInMeetingsDto>>(data);

                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TopUsersDto>> GetTopUsersData(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                var startDate = new DateTime(parameters.StartDate.Year, parameters.StartDate.Month,
                    parameters.StartDate.Day, 0, 0, 0);
                var endDate = new DateTime(parameters.EndDate.Year, parameters.EndDate.Month,
                    parameters.EndDate.Day, 23, 59, 59);
                var data = await _repositoryService.GetTopUsersData(startDate, endDate, appUser);

                if (data != null)
                {
                    return _mapper.Map<List<TopUsersDto>>(data);
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
                var monthlyList = new List<MonthTotalSeriesDto>();
                var idCounter = 0;

                var monthlyDates = MonthsBetween(parameters.StartDate, parameters.EndDate);

               foreach(var dtDate in monthlyDates)
                {
                    var monthlyData = await _repositoryService.GetMonthlyTotals(new DateTime(dtDate.Year, dtDate.Month, 1), appUser) ?? throw new InvalidDataException($"Unable to get monthly totals for date: {dtDate}");

                    var mtTotal = new MonthTotalSeriesDto
                    {
                        SeriesTotal = monthlyData.totalmincount ?? 0,
                        MonthDate = $"{dtDate.Month}/{dtDate.Year}",
                        Id = idCounter++,
                        FilterDate = dtDate.DateValue

                    };
                    if (!parameters.Filters.Any()) continue;
                    foreach (var filter in parameters.Filters)
                    {
                        var filterData =
                            await _repositoryService.FilteredMeetingTotalTime(new DateTime(dtDate.Year, dtDate.Month, 1),
                                filter, appUser) ?? throw new InvalidDataException(
                                $"Unable to get monthly totals for date: {dtDate} with filter {filter}");

                        var sum = filterData.Sum(s => s.Participantlength);

                        mtTotal.SeriesFiltered += sum ?? 0;

                    }

                    monthlyList.Add(mtTotal);
                }

                return monthlyList;
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TopUsersDto>> GetTopUsersDataFiltered(EmployeeMeetingParametersFilter parameters, string appUser)
        {
            try
            {
                var userList = new List<TopUsersDto>();

                //get all meetings first
                foreach (var filter in parameters.Filters)
                {
                    var users = await _repositoryService.FiServTopUsers(parameters, filter, appUser);

                    foreach (var user in users)
                    {
                        var inList = userList.FirstOrDefault(w => string.Equals(w.DisplayName, user.displayname, StringComparison.CurrentCultureIgnoreCase));

                        if (inList != null)
                        {
                            inList.EmployeeTotalTime = user.emptime ?? 0;
                        }
                        else
                        {
                            userList.Add(new TopUsersDto
                            {
                                DisplayName = user.displayname,
                                EmployeeTotalTime = user.emptime ?? 0,
                                Email = user.email
                            });
                        }
                    }
                }

                return userList.OrderByDescending(o => o.EmployeeTotalTime).Take(15).ToList();



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

                var data = await _repositoryService.GetEmployeesInMeeting(parameters.MeetingId, appUser);

                if (data != null)
                {
                    return _mapper.Map<List<MeetingParticipantDto>>(data);

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
                var data = await _repositoryService.GetEmployeeMeetings(parameters, appUser);

                if (data != null)
                {
                    var returnData = new List<EmployeeMeetingDto>();

                    foreach (var meeting in data)
                    {

                        var empMeeting = new EmployeeMeetingDto
                        {
                            EmployeeTime = meeting.EmployeeTime ?? 0,
                            Title = meeting.Title
                        };

                        if (meeting.EmployeeTime.HasValue)
                        {
                            empMeeting.PercentOfTotalTime =
                                meeting.EmployeeTime.Value / (decimal)parameters.TotalEmpTime;
                        }

                        if (meeting.Title.ToLower().Contains("dna") || meeting.Title.ToLower().Contains("fiserv") ||
                            meeting.Title.ToLower().Contains("catalyst") || meeting.Title.ToLower().Contains("nautilus") || meeting.Title.ToLower().Contains("weekly engagement") || parameters.DisplayName.ToLower().Contains("catalyst") ||
                            parameters.Email.ToLower().Contains("fiserv"))
                        {
                            empMeeting.Color = "#6F42C1";
                        }
                        else
                        {
                            empMeeting.Color = "#ff9411";
                        }

                        returnData.Add(empMeeting);

                    }

                    return returnData.OrderByDescending(o => o.EmployeeTime).Take(15).ToList();
                }

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<CompareTimeDto>> EmployeeMeetingTimeCompare(EmployeeMeetingsListParameters parameters, string appUser)
        {
            try
            {
                var returnList = new List<CompareTimeDto>();

                var fiServMeetings =
                    await _repositoryService.GetEmployeeFiServMeetingsTotalTime(parameters, appUser);

                var nonFiServeMeetings = await _repositoryService.GetEmployeeNonFiServMeetingsTotalTime(parameters, appUser);


                if (fiServMeetings is { Count: > 0 })
                {
                    var employeeTotalTime = fiServMeetings.FirstOrDefault()!.EmployeeTotalTime;
                    if (employeeTotalTime != null)
                    {
                        var totalTime = fiServMeetings.FirstOrDefault()!.EmployeeTotalTime;
                        if (totalTime != null)
                            returnList.Add(new CompareTimeDto
                            {
                                DisplayName = fiServMeetings.FirstOrDefault()?.DisplayName,
                                EmployeeTotalTime = employeeTotalTime.Value,
                                Color = "#6F42C1",
                                PercentOfTotalTime = totalTime.Value /
                                                     (decimal)parameters.TotalEmpTime
                            });
                    }
                }

                if (nonFiServeMeetings is not { Count: > 0 }) return returnList;
                {
                    if (fiServMeetings is { Count: > 0 })
                    {
                        var employeeTotalTime = fiServMeetings.FirstOrDefault()!.EmployeeTotalTime ?? 0;
                        returnList.Add(new CompareTimeDto
                        {
                            DisplayName = nonFiServeMeetings.FirstOrDefault()?.DisplayName,
                            EmployeeTotalTime = parameters.TotalEmpTime -
                                                employeeTotalTime,
                            Color = "#ff9411",
                            PercentOfTotalTime =
                                (parameters.TotalEmpTime - employeeTotalTime) /
                                (decimal)parameters.TotalEmpTime
                        });
                    }
                    else
                    {
                        returnList.Add(new CompareTimeDto
                        {
                            DisplayName = nonFiServeMeetings.FirstOrDefault()?.DisplayName,
                            EmployeeTotalTime = parameters.TotalEmpTime,
                            Color = "#ff9411",
                            PercentOfTotalTime =
                                parameters.TotalEmpTime /
                                (decimal)parameters.TotalEmpTime
                        });
                    }


                }

                return returnList;

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
                var data = await _repositoryService.GetEmployeeMeetingTotalTime(parameters, appUser);

                if (data != null)
                {
                    return _mapper.Map<EmployeeMeetingTotalTimeDto>(data.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        private async Task<bool> GetWebExApiMeetings(DateTime dateValue, string appUser)
        {
            try
            {

                var fromDt =
                    new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 00, 00, 00, DateTimeKind.Utc)
                        .ToString("s");

                var toDt = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 23, 59, 59,
                    DateTimeKind.Utc).ToString("s");

                var response = await _webExApiCalls.GetWebExData(
                    $"{_settings.Value.WebExApiBaseUrl}/meetings?meetingType=meeting&max=100&from={fromDt}&to{toDt}&state=ended",
                    appUser);

                if (response != null)
                {
                    var webExMeeting =
                        await response.Content.ReadFromJsonAsync<Meeting>();

                    var meetingsList = _mapper.Map<List<MeetingItem>>(webExMeeting.items);

                    var hasHeader = response.Headers.TryGetValues("Link", out _);

                    while (hasHeader)
                    {
                        if (!response.Headers.GetValues("Link").FirstOrDefault()!.Contains("rel=\"next\""))
                            continue;
                        var pageUrl = response.Headers.GetValues("Link").FirstOrDefault()?.Split(';');

                        response = await _webExApiCalls.GetWebExData(
                            (pageUrl ?? throw new InvalidOperationException(
                                "Link Page Url cannot be read from header")).FirstOrDefault()?.Replace("<", "")
                            .Replace(">", ""), appUser);

                        webExMeeting = await response.Content.ReadFromJsonAsync<Meeting>();

                        meetingsList.AddRange(_mapper.Map<List<MeetingItem>>(webExMeeting.items));

                        hasHeader = response.Headers.TryGetValues("Link", out _);
                    }


                    return await _repositoryService.AddMeetings(_mapper.Map<List<Meetings>>(meetingsList), appUser);
                }

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;

        }

        private async Task<bool> GetWebExApiMeetingParticipants(string meetingId, string hostEmail, string appUser)
        {
            try
            {



                var response = await _webExApiCalls.GetWebExData(
                    $"{_settings.Value.WebExApiBaseUrl}/meetingParticipants?meetingId={meetingId}&hostEmail={hostEmail}&max=100", appUser);

                if (response != null)
                {
                    var webExMeetingParticipants =
                        await response.Content.ReadFromJsonAsync<Models.WebExParticipant.Participant>();


                    var participantList = _mapper.Map<List<ParticipantItem>>(webExMeetingParticipants.items);

                    var hasHeader = response.Headers.TryGetValues("Link", out _);

                    while (hasHeader)
                    {
                        if (!response.Headers.GetValues("Link").FirstOrDefault()!.Contains("rel=\"next\"")) continue;
                        var pageUrl = response.Headers.GetValues("Link").FirstOrDefault()?.Split(';');

                        response = await _webExApiCalls.GetWebExData((pageUrl ?? throw new InvalidOperationException("Link Page Url cannot be read from header")).FirstOrDefault()?.Replace("<", "").Replace(">", ""), appUser);

                        webExMeetingParticipants = await response.Content.ReadFromJsonAsync<Models.WebExParticipant.Participant>();

                        participantList.AddRange(_mapper.Map<List<ParticipantItem>>(webExMeetingParticipants.items));

                        hasHeader = response.Headers.TryGetValues("Link", out _);
                    }


                    return await _repositoryService.AddParticipant(_mapper.Map<List<MeetingParticipants>>(participantList), appUser);



                }




            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;
        }

        private static IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
                return date;
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
}
