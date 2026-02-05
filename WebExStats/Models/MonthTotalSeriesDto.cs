namespace Stats.Models
{
    public class MonthTotalSeriesDto
    {
        public int Id { get; set; }
        public int SeriesTotal { get; set; }

        public int SeriesFiltered { get; set; }

        public string MonthDate { get; set; }

        public DateTime FilterDate { get; set; }
    }


}
