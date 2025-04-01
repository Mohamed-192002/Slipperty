using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class FollowUpDetails
    {
        public int OrderId { get; set; }
        public int ContactMethodId { get; set; } = 1;  // 1 phone ,  2 WhatsApp 
        public int ContactStatusId { get; set; } = 1; // 1 Not determine 
        public string? FollowUpNote { get; set; }
        public string? Notes { get; set; }
        
        [ForeignKey("OrderId")]
        public virtual OrderHead OrderHead { get; set; }
    }
}
