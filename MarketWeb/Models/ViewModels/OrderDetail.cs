using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Models.ViewModels
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public Product Products { get; set; }

        public Order Orders { get; set; }
    }
}
