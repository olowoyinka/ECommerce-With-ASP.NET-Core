using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Models.ViewModels
{
    public class Storage
    {
        public int StorageID { get; set; }

        [Required]
        [Display(Name = "Storage")]
        public string Storages { get; set; }
    }
}