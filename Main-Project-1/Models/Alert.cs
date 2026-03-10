public class Alert
{
    public int Id { get; set; }

    public int ApplianceId { get; set; }

    public string AlertType { get; set; }

    public string Severity { get; set; }

    public string Message { get; set; }

    public bool IsResolved { get; set; }

    public DateTime CreatedAt { get; set; }

    public Appliance Appliance { get; set; }

    public ICollection<Notification> Notifications { get; set; }
}