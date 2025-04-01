using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Migrations.Models
{
    public class OrderUrgentDetails : IModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual OrderHead OrderHead { get; set; } = null!;
        public DateTime UrgentDate { get; set; }
        public double? AdditionalPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn{ get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
