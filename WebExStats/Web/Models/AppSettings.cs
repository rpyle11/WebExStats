namespace Stats.Web.Models
{
    public class AppSettings
    {
        public string LogEmailSubject { get; set; }

        public string LogFromAddress { get; set; }

        public string LogToAddress { get; set; }

        public List<string> MonthlyTotalFilters { get; set; }

    }
}
