namespace Stats.Models
{
    public class MeetingItem
    {
        public string Id { get; set; }
        public string MeetingSeriesId { get; set; }
        public string ScheduledMeetingId { get; set; }
        public string Title { get; set; }
        public string MeetingType { get; set; }
        public string State { get; set; }
        public string Timezone { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string HostUserId { get; set; }
        public string HostDisplayName { get; set; }
        public string HostEmail { get; set; }
        public string SiteUrl { get; set; }
        public string ScheduledType { get; set; }
        public bool HasChat { get; set; }
        public bool HasRecording { get; set; }
        public bool HasTranscription { get; set; }
        public bool HasClosedCaption { get; set; }
        public bool HasPolls { get; set; }
        public bool HasQa { get; set; }
        public string MeetingNumber { get; set; }
        public bool EnabledAutoRecordMeeting { get; set; }
        public bool AllowAnyUserToBeCoHost { get; set; }
        public bool AllowFirstUserToBeCoHost { get; set; }
        public bool AllowAuthenticatedDevices { get; set; }
        public bool EnabledJoinBeforeHost { get; set; }
        public int JoinBeforeHostMinutes { get; set; }
        public bool EnableConnectAudioBeforeHost { get; set; }
        public bool ExcludePassword { get; set; }
        public bool PublicMeeting { get; set; }
        public bool EnableAutomaticLock { get; set; }
        public int SessionTypeId { get; set; }
        public int ReminderTime { get; set; }
    }
}
