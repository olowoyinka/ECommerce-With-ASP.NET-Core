using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Models.ViewModels
{
    public class Color
    {
        public int ColorID { get; set; }

        [Required]
        [Display(Name = "Color")]
        public string Colors { get; set; }
    }
}
