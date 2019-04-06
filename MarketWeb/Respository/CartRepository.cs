using MarketWeb.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Respository
{
    public class CartRepository : ICartRespository
    {
        private readonly IHttpContextAccessor _contextAccesor;

        public CartRepository(IHttpContextAccessor contextAccessor)
        {
            _contextAccesor = contextAccessor;

        }


        public bool IsExistingCheck(int? Id)
        {
            List<Cart> IsCart = _contextAccesor.HttpContext.Session.GetObjectFromJson<List<Cart>>("Cart");

            return IsCart.Any(p => p.Product.ProductID == Id);
        }
    }
}
