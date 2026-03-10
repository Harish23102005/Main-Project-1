namespace MainProject1.DTOs
{
    public class CreateAlertDto
    {
        public int ApplianceId { get; set; }

        public string AlertType { get; set; }

        public string Severity { get; set; }

        public string Message { get; set; }
    }
}