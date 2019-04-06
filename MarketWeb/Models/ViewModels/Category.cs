using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Models.ViewModels
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
