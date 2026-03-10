public class SensorData
{
    public int Id { get; set; }

    public int ApplianceId { get; set; }

    public DateTime Timestamp { get; set; }

    public string ReadingType { get; set; }

    public double Value { get; set; }

    public string Unit { get; set; }

    public Appliance Appliance { get; set; }
}