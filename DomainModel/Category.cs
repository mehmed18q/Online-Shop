using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("Category")]
    public class Category:ObjectModel
    {
        public Category() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Id { get; set; }
        [Column(Order = 2)]
        [Display(Name="نام به انگليسي")]
        public string Name { get; set; }
        [Column(Order = 3)]
        [Display(Name = "عنوان به فارسي")]
        public string Title { get; set; }
        public List<Product> Products { get; set; }
    }
}
