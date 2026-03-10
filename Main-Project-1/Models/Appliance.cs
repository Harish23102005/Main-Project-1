public class Appliance
{
    public int Id { get; set; }

    public int HomeId { get; set; }

    public int ApplianceTypeId { get; set; }   // ← IMPORTANT FIX

    public string DeviceIdentifier { get; set; }

    public string Name { get; set; }

    public string Model { get; set; }

    public string Status { get; set; }

    public DateTime InstalledAt { get; set; }

    public Home Home { get; set; }

    public ApplianceType ApplianceType { get; set; }
}