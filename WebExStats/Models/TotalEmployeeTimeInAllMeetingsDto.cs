namespace Stats.Models
{
    public class TotalEmployeeTimeInAllMeetingsDto
    {
        public string MeetingId { get; set; }
        public DateTime MeetingStart { get; set; }
        public DateTime MeetingEnd { get; set; }
        public string Title { get; set; }
      
        public int? EmpTotalTime { get; set; }
    }
}
