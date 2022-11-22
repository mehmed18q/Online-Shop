using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Order")]
    public class Order:ObjectModel
    {
        public Order() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int OrderStaus { get; set; }
        public OrderStatus orderStatus { get; set; }
        public string TotalPrice { get; set; }
        public List<OrderItems> orderItems { get; set; }
    }
}
