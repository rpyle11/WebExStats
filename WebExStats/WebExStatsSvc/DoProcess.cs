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

                var meetingResponse = await ApiCalls.GetMeetingsCall();

                if (meetingResponse)
                {
                    return await ApiCalls.GetMeetingParticipantsCall();

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

      
    }
}
