namespace Stats.Api.Models.WebExParticipant;

public class Participant
{
    public Item[] items { get; set; }
}

public class Item
{
    public string id { get; set; }
    public bool host { get; set; }
    public bool coHost { get; set; }
    public bool spaceModerator { get; set; }
    public string email { get; set; }
    public string displayName { get; set; }
    public bool invitee { get; set; }
    public bool muted { get; set; }
    public string state { get; set; }
    public DateTime? joinedTime { get; set; }
    public DateTime? leftTime { get; set; }
    public string siteUrl { get; set; }
    public string meetingId { get; set; }
    public string hostEmail { get; set; }
    public DateTime? meetingStartTime { get; set; }

}