using Stats.Api.Entities;
using Stats.Models;

namespace Stats.Api.Services;

public interface IRepositoryService
{
    Task<bool> AddMeetings(List<Meetings> meetings, string appUser);

    Task<bool> AddPullLog(DataPullLogs log, string appUser);

    Task<List<GetMeetingsNoParticipantsResult>> GetMeetingsPullParticipants(string appUser);

    Task<bool> AddParticipant(List<MeetingParticipants> participants, string appUser);

    Task<bool> UpdateMeetingParticipantsPulled(string id, string appUser);

    Task<LastDataPullResult> GetLastPullByType(string pullType, string appUser);

    Task<StartEndDatesResult> GetStartEndDates(string appUser);

    Task<List<AllEmployeesInAllMeetingsResult>> GetEmployeesInAllMeetings(EmployeeMeetingParameters parameters,
        string appUser);

    Task<List<TotalEmployeeTimeInAllMeetingsResult>> GetEmployeesTotalTimeInMeetings(
        EmployeeMeetingParameters parameters, string appUser);

    Task<List<ByEmployeeTotalTimeInMeetingsResult>> GetByEmployeesTotalMeetingsTime(DateTime startDate, DateTime endDate, string appUser);

    Task<List<TopUsersResult>> GetTopUsersData(DateTime startDate, DateTime endDate, string appUser);

    Task<TotalTimePerMonthResult> GetMonthlyTotals(DateTime startDate, string appUser);

    Task<List<TotalTimePerMonthView>> FilteredMeetingTotalTime(DateTime startDate, string filterValue, string appUser);

    Task<List<TopFiservUsersResult>> FiServTopUsers(EmployeeMeetingParametersFilter parameters, string filter,
        string appUser);

    

    Task<List<MeetingParticipantsListResult>> GetEmployeesInMeeting(string meetingId, string appUser);

    Task<List<EmployeeMeetingsResult>> GetEmployeeMeetings(EmployeeMeetingsListParameters parameters, string appUser);

    Task<List<EmployeeTotalFiServMeetingsResult>> GetEmployeeFiServMeetingsTotalTime(
        EmployeeMeetingsListParameters parameters, string appUser);

    Task<List<EmployeeTotalNonFiServMeetingsResult>> GetEmployeeNonFiServMeetingsTotalTime(
        EmployeeMeetingsListParameters parameters, string appUser);

    Task<List<EmployeeMeetingTotalTimeResult>> GetEmployeeMeetingTotalTime(
        EmployeeTotalTimeParameters parameters, string appUser);

    Task<List<Tokens>> GetWebExApiTokens(string appUser);

    Task<Tokens> UpdateWebExApiToken(Tokens token, string appUser);



}