namespace Stats.Models;

public class ParticipantItem
{
    public string Id { get; set; }
    public bool Host { get; set; }
    public bool CoHost { get; set; }
    public bool SpaceModerator { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public bool Invitee { get; set; }
    public bool Muted { get; set; }
    public string State { get; set; }
    public DateTime? JoinedTime { get; set; }
    public DateTime? LeftTime { get; set; }
    public string SiteUrl { get; set; }
    public string MeetingId { get; set; }
    public string HostEmail { get; set; }
    public DateTime? MeetingStartTime { get; set; }
}