namespace MainProject1.DTOs
{
    public class CreateWaterUsageDto
    {
        public int ApplianceId { get; set; }

        public double LitersConsumed { get; set; }

        public int CycleCount { get; set; }

        public double CostEstimate { get; set; }
    }
}