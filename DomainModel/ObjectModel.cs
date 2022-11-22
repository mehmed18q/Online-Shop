using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel
{
    public class ObjectModel
    {
        [Column(Order = 7)]
        public DateTime? CreateDate { get; set; }
        [Column(Order = 8)]
        [DefaultValue(null)]
        public int? CreateBy { get; set; }
        [Column(Order = 9)]
        [DefaultValue(null)]
        public DateTime? EditDate { get; set; }
        [Column(Order = 10)]
        [DefaultValue(null)]
        public int? EditBy { get; set; }
        [Column(Order = 11)]
        [DefaultValue(false)]
        public bool IsDelete { get; set; }
    }
}
