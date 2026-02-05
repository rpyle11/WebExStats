namespace Stats.Models
{
    public  class EmployeesAllMeetingsDto
    {
        public long DbId { get; set; }
        public string MeetingId { get; set; }
        public string DisplayName { get; set; }

        public string Email { get; set; }
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? JoinedTime { get; set; }
        public DateTime? LeftTime { get; set; }
    }
}
