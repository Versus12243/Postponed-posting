using Postponed_posting.Common.Extentions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Social_network
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Access_tokens> Access_tokens { get; set; }

        public Social_network()
        {
            Access_tokens = Access_tokens.Empty();
        }
    }
}
