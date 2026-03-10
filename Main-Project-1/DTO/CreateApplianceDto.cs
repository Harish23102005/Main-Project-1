namespace MainProject1.DTOs
{
    public class CreateApplianceDto
    {
        public int HomeId { get; set; }

        public int TypeId { get; set; }

        public string DeviceIdentifier { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public string Status { get; set; }
    }
}