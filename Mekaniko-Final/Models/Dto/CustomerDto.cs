namespace Mekaniko_Final.Models.Dto
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public bool? PaymentStatus { get; set; }
        public List<CarDto> Car { get; set; }
    }
}
