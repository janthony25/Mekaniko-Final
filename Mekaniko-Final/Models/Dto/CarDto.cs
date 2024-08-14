namespace Mekaniko_Final.Models.Dto
{
    public class CarDto
    {
        public int CarId { get; set; }
        public string CarRego { get; set; }
        public string CarModel { get; set; }
        public string CarYear { get; set; }
        public bool? CarPaymentStatus { get; set; }
        public List<MakeDto> MakeName { get; set; }
    }
}
