using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models.Online;

namespace TIPMC_WebApp_Vesperr.ViewComponents
{
    public class SmallCartViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SmallCartViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync_(string memberid)
        {

            // Retrieve cart items from your data source or wherever you have them stored
            List<CartItem> cart = await _context.Cart.ToListAsync();

            SmallCartViewModel smallCartVM;

            if (cart == null || cart.Count == 0)
            {
                smallCartVM = null;
            }
            else
            {
                smallCartVM = new()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmount = cart.Sum(x => x.Quantity * x.Price)
                };
            }

            return View(smallCartVM);
        }

        public async Task<IViewComponentResult> InvokeAsync(string memberid)
        {
            // Replace the following lines with your actual logic
            string memberIdValue = memberid;

            IQueryable<CartItem> cartQuery = _context.Cart;

            if (!string.IsNullOrEmpty(memberIdValue))
            {
                // If memberid is provided, filter cart items based on memberid
                cartQuery = cartQuery.Where(cartItem => cartItem.MemberId == memberIdValue);
            }

            // Retrieve cart items from your data source or wherever you have them stored
            List<CartItem> cart = await cartQuery.ToListAsync();

            SmallCartViewModel smallCartVM;

            if (cart == null || cart.Count == 0)
            {
                smallCartVM = null;
            }
            else
            {
                smallCartVM = new()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmount = cart.Sum(x => x.Quantity * x.Price)
                };
            }

            return View(smallCartVM);
        }
    }
}
