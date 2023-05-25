using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("Order")]
    public class Order : ObjectModel
    {
        public Order()
        {
            UserId = 0;
            User = new User();
            OrderStatusId = 0;
            OrderStatus = new OrderStatus();
            TotalPrice = string.Empty;
            OrderItems = new List<OrderItem>();
        }
        public int UserId { get; set; }
        public User User { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}