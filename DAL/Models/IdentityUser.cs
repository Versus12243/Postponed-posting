using Microsoft.AspNet.Identity.EntityFramework;
using Postponed_posting.Common.Extentions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{ 
     public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Social_network> Social_networks { get; set; }          

        public ApplicationUser()
        {
            Social_networks = Social_networks.Empty();
        }
    }
}
