namespace WebExStatsSvc.Models
{
    public class AppLog
    {
        public string LogMsg { get; set; }

        public MessageTypeEnum MessageType { get; set; }

        public bool SendEmail { get; set; }

        public enum MessageTypeEnum
        {
            Error, Warning, Message
        }
    }
}
