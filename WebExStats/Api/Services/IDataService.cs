using Stats.Models;

namespace Stats.Api.Services;

public interface IDataService
{
    Task<bool> PullMeetings(MeetingParameters parameters, string appUser);

    Task<bool> PullMeetingParticipants(string appUser);

    Task<LastDataPullDto> GetLastDataPullByType(LastDataPullParameters parameters, string appUser);

    Task<StartEndDatesDto> GetStartEndDates(string appUser);

    Task<List<EmployeesAllMeetingsDto>> GetAllEmployeesInAllMeetings(EmployeeMeetingParameters parameters, string appUser);

    Task<List<TotalEmployeeTimeInAllMeetingsDto>> GetEmployeeTotalTimeInAllMeetings(EmployeeMeetingParameters parameters, string appUser);

    Task<List<ByEmployeeTotalTimeInMeetingsDto>> GetByEmployeeMeetingsTotalTime(EmployeeMeetingParameters parameters,
        string appUser);

    Task<List<TopUsersDto>> GetTopUsersData(EmployeeMeetingParameters parameters, string appUser);

    Task<List<MonthTotalSeriesDto>> GetMonthlyTotals(MonthlyTotalsParameters parameters, string appUser);

    Task<List<TopUsersDto>> GetTopUsersDataFiltered(EmployeeMeetingParametersFilter parameters, string appUser);

    Task<List<MeetingParticipantDto>> GetMeetingParticipants(MeetingEmployeesParameters parameters, string appUser);

    Task<List<EmployeeMeetingDto>> GetEmployeeMeetings(EmployeeMeetingsListParameters parameters, string appUser);

    Task<List<CompareTimeDto>> EmployeeMeetingTimeCompare(EmployeeMeetingsListParameters parameters, string appUser);

    Task<EmployeeMeetingTotalTimeDto> GetEmployeeMeetingTotalTime(
        EmployeeTotalTimeParameters parameters, string appUser);


}