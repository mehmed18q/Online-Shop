using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("Category")]
    public class Category : ObjectModel
    {
        public Category()
        {
            Image = string.Empty;
            Name = string.Empty;
            Title = string.Empty;
        }
        [Column(Order = 2), Display(Name = "نام به انگليسي")]
        public string Name { get; set; }
        [Column(Order = 3), Display(Name = "عنوان به فارسي")]
        public string Title { get; set; }
        public string? Image { get; set; }
        public List<Product>? Products { get; set; }
    }
}