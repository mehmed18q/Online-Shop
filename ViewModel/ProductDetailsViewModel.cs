using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "دسته بندی")]
        public string? CategoryTitle { get; set; }
        public int BrandId { get; set; }
        [Display(Name = "برند")]
        public string? BrandTitle { get; set; }
        [Display(Name = "عنوان")]
        public string? Title { get; set; }
        [Display(Name = "قیمت")]
        public string? Price { get; set; }
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
        [Display(Name = "تعداد")]
        public int? Quantity { get; set; }
        [Display(Name = "تصویر")]
        public string? MainImageTitle { get; set; }
    }
}