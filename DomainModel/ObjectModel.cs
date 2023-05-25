using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    public class ObjectModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 7)]
        public DateTime CreatedAt { get; set; }

        [Column(Order = 8)]
        public int CreatedBy { get; set; }

        [Column(Order = 9), DefaultValue(null)]
        public DateTime? EditedAt { get; set; }

        [Column(Order = 10), DefaultValue(null)]
        public int? EditedBy { get; set; }

        [Column(Order = 11), DefaultValue(false)]
        public bool IsDelete { get; set; }

        [Column(Order = 12), DefaultValue(null)]
        public DateTime? DeletedAt { get; set; }

        [Column(Order = 13), DefaultValue(null)]
        public int? DeletedBy { get; set; }
    }
}