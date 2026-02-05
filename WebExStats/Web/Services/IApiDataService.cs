using Stats.Models;

namespace Stats.Web.Services;

public interface IApiDataService
{
    Task<StartEndDatesDto> GetStatisticDateRange(string appUser);

    Task<List<EmployeesAllMeetingsDto>> GetEmployeesAllMeetings(EmployeeMeetingParameters parameters, string appUser);

    Task<List<TotalEmployeeTimeInAllMeetingsDto>> GetEmployeeTotalTimeInMeetings(EmployeeMeetingParameters parameters, string appUser);

    Task<List<ByEmployeeTotalTimeInMeetingsDto>> GetByEmployeeTotalTimeInMeetings(EmployeeMeetingParameters parameters, string appUser);

    Task<List<TopUsersDto>> GetTopUsers(EmployeeMeetingParameters parameters, string appUser);

    Task<List<MonthTotalSeriesDto>> GetMonthlyTotals(MonthlyTotalsParameters parameters, string appUser);

    Task<List<TopUsersDto>> GetTopFiServUsers(EmployeeMeetingParametersFilter parameters, string appUser);

   

    Task<List<MeetingParticipantDto>> GetMeetingParticipants(MeetingEmployeesParameters parameters, string appUser);

    Task<List<EmployeeMeetingDto>> GetEmployeeMeetings(EmployeeMeetingsListParameters parameters, string appUser);

    Task<List<CompareTimeDto>>
        GetEmployeeMeetingsTimeCompare(EmployeeMeetingsListParameters parameters, string appUser);

    Task<EmployeeMeetingTotalTimeDto> GetEmployeeMeetingTotalTime(
        EmployeeTotalTimeParameters parameters, string appUser);

}