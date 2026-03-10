public class ApplianceType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Category { get; set; }

    public double AvgEnergyRating { get; set; }

    public double AvgWaterRating { get; set; }

    public ICollection<Appliance> Appliances { get; set; }
}