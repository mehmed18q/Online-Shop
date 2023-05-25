using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public OrderStatus() { 
        
            Id = 0;
            Name = string.Empty;
            Title = string.Empty;
            Orders = new List<Order>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public List<Order> Orders { get; set; }
    }
}