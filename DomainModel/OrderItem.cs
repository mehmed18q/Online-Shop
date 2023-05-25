using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("OrderItems")]
    public class OrderItem : ObjectModel
    {
        public OrderItem() {
            OrderId = 0;
            Order = new Order();
            ProductId = 0;
            Product = new Product();
            Quantity = 0;
            UnitPrice = string.Empty;
        }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
}