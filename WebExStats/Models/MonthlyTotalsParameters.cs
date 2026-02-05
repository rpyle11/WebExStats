namespace Stats.Models
{
    public class MonthlyTotalsParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<string> Filters { get; set; }
    }
}
