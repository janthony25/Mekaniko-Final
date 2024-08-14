namespace Mekaniko_Final.Models.Dto
{
    public class GetCustomerCarByCarIdDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public int CarId { get; set; }
        public string CarRego { get; set; }
        public string CarModel { get; set; }
        public string CarYear { get; set; }
        public string MakeName { get; set; }
    }
}
