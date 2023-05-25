using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("ProductImages")]
    public class ProductImage : ObjectModel
    {
        public ProductImage()
        {
            ProductId = 0;
            Product = new Product();
            Title = string.Empty;
            IsMain = false;
        }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Title { get; set; }
        public bool IsMain { get; set; }
    }
}