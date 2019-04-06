using MarketWeb.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Models.ViewModels
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        
        [Display(Name = "Image")]
        public string Image { get; set; }

        
        [Display(Name = "Self Start Date")]
        public DateTime SelfStartDate { get; set; }

        
        [Display(Name = "Self End Date")]
        public DateTime SelfEndDate { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Color")]
        public int ColorID { get; set; }

        [Required]
        [Display(Name = "Model")]
        public int ModelID { get; set; }

        [Required]
        [Display(Name = "Storage")]
        public int StorageID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Category Category { get; set; }

        public Color Color { get; set; }

        public Model Model { get; set; }

        public Storage Storage { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
