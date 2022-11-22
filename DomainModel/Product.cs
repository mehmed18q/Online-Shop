using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Product")]
    public class Product:ObjectModel
    {
        public Product() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Id { get; set; }
        [Column(Order = 2)]
        [DisplayName("دسته بندی")]
        public int CategoryId { get; set; }
        [DisplayName("دسته بندی")]
        public virtual Category Category { get; set; }
        [Column(Order =3)]
        [DisplayName("عنوان")]
        public string Title { get; set; }
        [Column(Order = 4)]
        [DisplayName("موجودی انبار")]
        public int Quantity { get; set; }
        [Column(Order = 5)]
        [DisplayName("قیمت واحد")]
        public string Price { get; set; }
        [Column(Order = 6)]
        [DisplayName("توضیحات")]
        public string Description { get; set; }
        public List<ProductFeatures> productFeatures { get; set; }
        public List<ProductImages> productImages { get; set; }
        public List<OrderItems> orderItems { get; set; }

    }
}
