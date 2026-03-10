public class EnergyUsage
{
    public int Id { get; set; }

    public int ApplianceId { get; set; }

    public DateTime Date { get; set; }

    public double KwhConsumed { get; set; }

    public double PeakUsage { get; set; }

    public double CostEstimate { get; set; }

    public Appliance Appliance { get; set; }
}