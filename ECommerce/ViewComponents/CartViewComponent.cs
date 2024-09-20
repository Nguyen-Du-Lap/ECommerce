using ECommerce.Helpers;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ECommerce.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItemVM>>(MySetting.CART_KEY) 
                ?? new List<CartItemVM>();
          
            return View("CartPanel", new CartVM
            {
                Quantity = cart.Sum(p => p.SoLuong),
                Total = cart.Sum(p => p.ThanhTien)
            });
        }
    }
}
