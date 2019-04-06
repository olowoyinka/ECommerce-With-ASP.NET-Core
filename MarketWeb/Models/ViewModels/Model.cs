using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Models.ViewModels
{
    public class Model
    {
        public int ModelID { get; set; }

        [Required]
        [Display(Name = "Models")]
        public string Models { get; set; }
    }
}
