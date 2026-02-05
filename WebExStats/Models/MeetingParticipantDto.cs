namespace Stats.Models
{
    public class MeetingParticipantDto
    {
        public long DbId { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public int? ParticipantTime { get; set; }
    }
}
