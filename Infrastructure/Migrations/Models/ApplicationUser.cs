using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public int? GovernmentId { get; set; }
        public int? RegionId { get; set; }

        public string Address { get; set; }

        public bool AutoRegistered { get; set; } = false;


        [ForeignKey(nameof(GovernmentId))]
        public Government? Government { get; set; }
        [ForeignKey(nameof(RegionId))]
        public Region? Region { get; set; }

    }
}
