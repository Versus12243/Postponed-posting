using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class LinkViewModel: BaseLinkViewModel
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public int GroupId { get; set; }
    }
}
