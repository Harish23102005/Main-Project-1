namespace MainProject1.DTOs
{
    public class CreateEnergyUsageDto
    {
        public int ApplianceId { get; set; }

        public double KwhConsumed { get; set; }

        public double PeakUsage { get; set; }

        public double CostEstimate { get; set; }
    }
}