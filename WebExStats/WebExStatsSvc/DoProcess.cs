using System;
using System.Reflection;
using System.Threading.Tasks;
using WebExStatsSvc.Models;
using WebExStatsSvc.Services;

namespace WebExStatsSvc
{
    public static class DoProcess
    {
        public static async Task<bool> PullData()
        {
            try
            {
                var paras = await SetMeetingParameters();

                if (paras != null)
                {
                    var meetingResponse = await ApiCalls.GetMeetingsCall(paras);

                    if (meetingResponse)
                    {
                        return await ApiCalls.GetMeetingParticipantsCall();

                    }
                }
                else
                {
                    throw new ApplicationException("Unable to set parameters for WebEx api calls");
                }

                return true;
            }
            catch (Exception ex)
            {
                await AppLogSvc.LogAlert(new AppLog
                {
                    LogMsg =
                        $"{ex.GetType().Name} Error in {MethodName.GetMethodName(MethodBase.GetCurrentMethod())} Error: {ex.Message}",
                    MessageType = AppLog.MessageTypeEnum.Error,
                    SendEmail = true
                });
            }

            return false;
        }

        private static async Task<MeetingParameters> SetMeetingParameters()
        {
            var parameters = new MeetingParameters
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };

            //1. if from and to dates are set on config - this is used for populating old data

            if (!string.IsNullOrEmpty(Properties.Settings.Default.FromDate) &&
                !string.IsNullOrEmpty(Properties.Settings.Default.ToDate))
            {
                var fromIsDate = DateTime.TryParse(Properties.Settings.Default.FromDate, out var dtF);
                var toIsDate = DateTime.TryParse(Properties.Settings.Default.ToDate, out var dtT);

                if (fromIsDate && toIsDate)
                {
                    parameters.FromDate = dtF;
                    parameters.ToDate = dtT;
                }
                else
                {
                    return parameters;
                }
            }
            else
            {
                //if nothing is set need to check for a last pull date
                var lastPull = await ApiCalls.GetLastPull();

                if (!lastPull.PullDate.HasValue) return parameters;  //if nothing is set just use values set when declaring variable
                parameters.FromDate = lastPull.PullDate.Value;
                parameters.ToDate = DateTime.Now;

            }

            //if nothing is set just use values set when declaring variable
            return parameters;
            
        }
    }
}
