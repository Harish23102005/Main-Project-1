public class SustainabilityScore
{
    public int Id { get; set; }

    public int HomeId { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public double EnergyScore { get; set; }

    public double WaterScore { get; set; }

    public double OverallScore { get; set; }

    public Home Home { get; set; }
}