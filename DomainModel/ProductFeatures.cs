using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel
{
    [Table("ProductFeatures")]
    public class ProductFeatures:ObjectModel
    {
        public ProductFeatures() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string FeatureName { get; set; }
        public string FeatureValue { get; set; }
    }
}
