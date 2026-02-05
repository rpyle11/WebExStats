namespace Stats.Models
{
    public class AppLog
    {
        public string LogMsg { get; set; }

        public string AppUser { get; set; }

        public MessageTypeEnum MessageType { get; set; }

        public bool SendEmail { get; set; }

        public enum MessageTypeEnum
        {
            Error, Message
        }

    }
}
