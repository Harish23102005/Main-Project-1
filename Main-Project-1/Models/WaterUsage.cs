public class WaterUsage
{
    public int Id { get; set; }

    public int ApplianceId { get; set; }

    public DateTime Date { get; set; }

    public double LitersConsumed { get; set; }

    public int CycleCount { get; set; }

    public double CostEstimate { get; set; }

    public Appliance Appliance { get; set; }
}