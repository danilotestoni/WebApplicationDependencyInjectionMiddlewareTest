namespace WebApplicationPostMeeting
{
    public class OrderModel
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public double? OrderCostTotal { get; set; }
    }
}
