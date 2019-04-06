using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Models.ViewModels
{
    public class ContactUs
    {
        public int ContactUsID { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Names { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Emails { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phones { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Contents { get; set; }
    }
}
