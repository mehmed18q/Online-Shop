using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("ProductFeatures")]
    public class ProductFeature : ObjectModel
    {
        public ProductFeature()
        {
            ProductId = 0;
            Product = new Product();
            FeatureName = string.Empty;
            FeatureValue = string.Empty;
        }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string FeatureName { get; set; }
        public string FeatureValue { get; set; }
    }
}