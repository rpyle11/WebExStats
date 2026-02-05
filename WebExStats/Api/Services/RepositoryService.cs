using Microsoft.EntityFrameworkCore;
using Stats.Api.Entities;
using Stats.Models;
using System.Reflection;
using Exception = System.Exception;

namespace Stats.Api.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly WebExStatsContext _context;
        private readonly ILogService _logService;

        public RepositoryService(WebExStatsContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public async Task<List<GetMeetingsNoParticipantsResult>> GetMeetingsPullParticipants(string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                return await spContext.GetMeetingsNoParticipantsAsync();

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<bool> UpdateMeetingParticipantsPulled(string id, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);
                await spContext.SetParticipantsPulledAsync(id);

                return true;

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;
        }

        public async Task<bool> AddParticipant(List<MeetingParticipants> participants, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                foreach (var participant in participants)
                {
                    await spContext.AddParticipantAsync(participant.Id, participant.Host, participant.CoHost,
                        participant.SpaceModerator, participant.Email, participant.DisplayName, participant.Invitee,
                        participant.Muted, participant.State, participant.JoinedTime, participant.LeftTime,
                        participant.SiteUrl, participant.MeetingId, participant.HostEmail,
                        participant.MeetingStartTime);
                }

                return true;
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;
        }

        public async Task<bool> AddMeetings(List<Meetings> meetings, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                foreach (var meeting in meetings.Where(w => w.State=="ended"))
                {
                    await spContext.AddMeetingAsync(meeting.Id, meeting.MeetingSeriesId, meeting.ScheduledMeetingId,
                        meeting.Title, meeting.MeetingType, meeting.State, meeting.Timezone, meeting.Start, meeting.End,
                        meeting.HostUserId, meeting.HostDisplayName, meeting.HostEmail, meeting.SiteUrl,
                        meeting.ScheduledType, meeting.HasChat, meeting.HasRecording, meeting.HasTranscription,
                        meeting.HasClosedCaption, meeting.HasPolls, meeting.HasQa, meeting.MeetingNumber,
                        meeting.EnabledAutoRecordMeeting, meeting.AllowAnyUserToBeCoHost,
                        meeting.AllowFirstUserToBeCoHost, meeting.AllowAuthenticatedDevices,
                        meeting.EnabledJoinBeforeHost, meeting.JoinBeforeHostMinutes,
                        meeting.EnableConnectAudioBeforeHost, meeting.ExcludePassword, meeting.PublicMeeting,
                        meeting.EnableAutomaticLock, meeting.SessionTypeId, meeting.ReminderTime);
                }

                return true;
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;
        }

        public async Task<bool> AddPullLog(DataPullLogs log, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                await spContext.AddPullLogAsync(log.PullType, log.PullDate, log.Parameters);

                return true;
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return false;
        }

        public async Task<LastDataPullResult> GetLastPullByType(string pullType, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                var data = await spContext.LastDataPullAsync(pullType);

                if(data != null)
                {
                    return data.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<StartEndDatesResult> GetStartEndDates(string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                var data = await spContext.StartEndDatesAsync();

                if (data != null)
                {
                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<AllEmployeesInAllMeetingsResult>> GetEmployeesInAllMeetings(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                var data = await spContext.AllEmployeesInAllMeetingsAsync(parameters.StartDate, parameters.EndDate);

                if (data != null)
                {
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TotalEmployeeTimeInAllMeetingsResult>> GetEmployeesTotalTimeInMeetings(EmployeeMeetingParameters parameters, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                var data = await spContext.TotalEmployeeTimeInAllMeetingsAsync(parameters.StartDate, parameters.EndDate);

                if (data != null)
                {
                    return data.Where(w => w.empTotalTime > 0).ToList();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<ByEmployeeTotalTimeInMeetingsResult>> GetByEmployeesTotalMeetingsTime(DateTime startDate, DateTime endDate, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                var data = await spContext.ByEmployeeTotalTimeInMeetingsAsync(startDate, endDate);

                if (data != null)
                {
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TopUsersResult>> GetTopUsersData(DateTime startDate, DateTime endDate, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);



                var data = await spContext.TopUsersAsync(startDate, endDate);

                if (data != null)
                {
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<TotalTimePerMonthResult> GetMonthlyTotals(DateTime startDate, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                var data = await spContext.TotalTimePerMonthAsync(startDate);

                if (data != null)
                {
                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TotalTimePerMonthView>> FilteredMeetingTotalTime(DateTime startDate, string filterValue, string appUser)
        {
            try
            {
                var data = await _context.TotalTimePerMonthView.Where(w => w.Startmonth >= startDate && w.Startmonth <= startDate && w.Title.ToLower().Contains(filterValue.ToLower())
                ).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<TopFiservUsersResult>> FiServTopUsers(EmployeeMeetingParametersFilter parameters,
           string filter, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                return await spContext.TopFiservUsersAsync(filter, parameters.StartDate, parameters.EndDate);
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

       

        public async Task<List<MeetingParticipantsListResult>> GetEmployeesInMeeting(string meetingId, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                var data = await spContext.MeetingParticipantsListAsync(meetingId);

                if (data != null)
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }


        public async Task<List<EmployeeMeetingsResult>> GetEmployeeMeetings(EmployeeMeetingsListParameters parameters, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                return await spContext.EmployeeMeetingsAsync(parameters.Email,parameters.DisplayName, parameters.StartDate,
                    parameters.EndDate);
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<EmployeeTotalFiServMeetingsResult>> GetEmployeeFiServMeetingsTotalTime(EmployeeMeetingsListParameters parameters, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                return await spContext.EmployeeTotalFiServMeetingsAsync(parameters.Email, parameters.DisplayName, parameters.StartDate,
                    parameters.EndDate);
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<EmployeeTotalNonFiServMeetingsResult>> GetEmployeeNonFiServMeetingsTotalTime(EmployeeMeetingsListParameters parameters, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);

                return await spContext.EmployeeTotalNonFiServMeetingsAsync(parameters.Email, parameters.DisplayName, parameters.StartDate,
                    parameters.EndDate);
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<EmployeeMeetingTotalTimeResult>> GetEmployeeMeetingTotalTime(
            EmployeeTotalTimeParameters parameters, string appUser)
        {
            try
            {
                var spContext = new WebExStatsContextProcedures(_context);


                return await spContext.EmployeeMeetingTotalTimeAsync(parameters.StartDate, parameters.EndDate,
                    parameters.Email, parameters.DisplayName);


            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<List<Tokens>> GetWebExApiTokens(string appUser)
        {
            try
            {
                return await _context.Tokens.ToListAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

        public async Task<Tokens> UpdateWebExApiToken(Tokens token, string appUser)
        {
            try
            {
                var inDb = await _context.Tokens.FirstOrDefaultAsync(f => f.Id==token.Id);

                if (inDb != null)
                {
                    inDb.Token = token.Token;
                    _context.Tokens.Update(inDb);
                    await _context.SaveChangesAsync();

                    return inDb;
                }
            }
            catch (Exception ex)
            {
                await _logService.LogAlert(AppLogPrep.AppErrorLogSetup(appUser, MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex));
            }

            return null;
        }

    }
}
