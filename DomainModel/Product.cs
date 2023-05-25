using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("Product")]
    public class Product : ObjectModel
    {
        public Product()
        {
            CategoryId = 0;
            Category = new Category();
            BrandId = 0;
            Brand = new Brand();
            Title = string.Empty;
            Price = string.Empty;
            Description = string.Empty;
            Quantity = 0;
            ProductFeatures = new List<ProductFeature>();
            ProductImages = new List<ProductImage>();
            OrderItems = new List<OrderItem>();
        }

        [Column(Order = 2), DisplayName("دسته بندی")]
        public int CategoryId { get; set; }
        [DisplayName("دسته بندی")]
        public virtual Category Category { get; set; }
        [Column(Order = 3), DisplayName("برند")]
        public int BrandId { get; set; }
        [DisplayName("برند")]
        public virtual Brand Brand { get; set; }
        [Column(Order = 4), DisplayName("عنوان")]
        public string Title { get; set; }
        [Column(Order = 5), DisplayName("موجودی انبار")]
        public int Quantity { get; set; }
        [Column(Order = 6), DisplayName("قیمت واحد")]
        public string Price { get; set; }
        [Column(Order = 14), DisplayName("توضیحات")]
        public string Description { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}