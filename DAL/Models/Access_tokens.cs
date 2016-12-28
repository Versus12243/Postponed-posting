using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Access_tokens
    {
        [Key]
        public int Id { get; set; }

        public virtual Social_network Social_network { get; set; }
    }
}
