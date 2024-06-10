namespace MessageExchanger.Abstractions.Models
{
    public struct DateRange
    {
        public DateRange(string startDate, string endDate) : this()
        {
            if (string.IsNullOrWhiteSpace(startDate))
            {
                throw new ArgumentNullException("startDate");
            }

            if (string.IsNullOrWhiteSpace(endDate))
            {
                throw new ArgumentNullException("endDate");
            }

            Start = DateTime.Parse(startDate);
            End = DateTime.Parse(endDate);

            if (End < Start)
            {
                throw new ArgumentException("endDate must be greater than or equal to startDate");
            }
        }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            Start = startDate;
            End = endDate;

            if (End < Start)
            {
                throw new ArgumentException("endDate must be greater than or equal to startDate");
            }
        }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }
    }
}
