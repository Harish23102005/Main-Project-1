public class Home
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; }

    public ICollection<Appliance> Appliances { get; set; }

    public ICollection<SustainabilityScore> SustainabilityScores { get; set; }
}