public class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AlertId { get; set; }

    public string Channel { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime? ReadAt { get; set; }

    public User User { get; set; }

    public Alert Alert { get; set; }
}