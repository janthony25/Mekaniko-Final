namespace Mekaniko_Final.Models.Dto
{
    public class UpdateInvoiceItemDto
    {
        public int InvoiceItemId { get; set; }
        public required string ItemName { get; set; }
        public required int Quantity { get; set; }
        public required decimal ItemPrice { get; set; }
        public decimal? ItemTotal { get; set; }
    }
}
