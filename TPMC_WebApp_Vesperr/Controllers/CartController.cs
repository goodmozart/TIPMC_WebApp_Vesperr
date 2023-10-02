using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models.Online;
using TIPMC_WebApp_Vesperr.Models.POS;

namespace TIPMC_WebApp_Vesperr.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string memberid)
        {
            IQueryable<CartItem> cartQuery = _context.Cart;

            if (!string.IsNullOrEmpty(memberid))
            {
                // If memberid is provided, filter cart items based on memberid
                cartQuery = cartQuery.Where(cartItem => cartItem.MemberId == memberid);

            }

            List<CartItem> cartItems = await cartQuery.ToListAsync();

            decimal grandTotal = cartItems.Sum(item => item.Quantity * item.Price);

            var cartViewModel = new CartViewModel
            {
                CartItems = cartItems,
                GrandTotal = grandTotal
                // Calculate and set the GrandTotal
            };

            return View(cartViewModel);
        }
        public ApplicationDbContext Get_context()
        {
            return _context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCartItem(long id, string memberId)
        {
            ProductOnline product = await _context.Products.FindAsync(id);

            //CartItem cartItem = _context.Cart
            //    .Where(c => c.ProductId == id)
            //    .Select(c => new CartItem
            //    {
            //        ProductId = c.ProductId,
            //        ProductName = c.ProductName,
            //        Quantity = c.Quantity,
            //        Price = c.Price,
            //        Image = c.Image,
            //        MemberId = c.MemberId
            //    })
            //    .FirstOrDefault();

            CartItem cartItem = _context.Cart
            .FromSqlRaw("SELECT * FROM dbo.Cart WHERE ProductId = {0} AND MemberId = {1}", id, memberId)
            .FirstOrDefault();

            if (cartItem == null)
            {
                // Create a new cart item
                cartItem = new CartItem
                {
                    ProductId = id,
                    ProductName = product.Name,
                    Quantity = 1, // Initial quantity
                    Price = product.Price,
                    Image = product.Image,
                    MemberId = memberId,
                };
                _context.Add(cartItem);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                string sqlQuery = "UPDATE dbo.Cart SET Quantity = '" + (cartItem.Quantity += 1) + "' WHERE ProductId = '" + id + "' AND MemberId = '" + memberId + "'";
                int affectedRows = _context.Database.ExecuteSqlRaw(sqlQuery);
            }


            return RedirectToAction("Index", "ProductOnline", new { memberId });

        }
        // POST: Cart/Add
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(long id, string memberid)
        {
            ProductOnline product = await _context.Products.FindAsync(id);
            CartItem cartItem = await _context.Cart
           .FirstOrDefaultAsync(m => m.ProductId == id && m.MemberId == memberid);

            if (ModelState.IsValid)
            {
                if (cartItem != null)
                {
                    string sqlQuery = "UPDATE dbo.Cart SET Quantity = '" + (cartItem.Quantity += 1) + "' WHERE ProductId = '" + id + "' AND MemberId = '" + memberid + "'";
                    int affectedRows = _context.Database.ExecuteSqlRaw(sqlQuery);
                }

                TempData["Success"] = "The product has been added!";
                return RedirectToAction(nameof(Index), new { memberid });
            }

            return View(cartItem);
        }

        // POST: Cart/Add
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Minus(long id, string memberid)
        {
            ProductOnline product = await _context.Products.FindAsync(id);
            CartItem cartItem = await _context.Cart
           .FirstOrDefaultAsync(m => m.ProductId == id && m.MemberId == memberid);

            if (ModelState.IsValid)
            {
                if (cartItem != null)
                {
                    if (cartItem.Quantity > 0)
                    {
                        string sqlQuery = "UPDATE dbo.Cart SET Quantity = '" + (cartItem.Quantity -= 1) + "' WHERE ProductId = '" + id + "' AND MemberId = '" + memberid + "'";
                        int affectedRows = _context.Database.ExecuteSqlRaw(sqlQuery);
                    }
                }

                TempData["Success"] = "The product has been removed!";
                return RedirectToAction(nameof(Index), new { memberid });

            }

            return View(cartItem);
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(long? id, string memberid)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cartItem = await _context.Cart
           .FirstOrDefaultAsync(m => m.ProductId == id && m.MemberId == memberid);

            if (cartItem == null)
            {
                return NotFound();
            }
            return View(cartItem);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }
        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cartItem = await _context.Cart.FindAsync(id);
            if (cartItem != null)
            {
                _context.Cart.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CartItemExists(long id)
        {
            return _context.Cart.Any(e => e.ProductId == id);
        }

        public async Task<IActionResult> ClearAsync(string memberid)
        {

            CartItem cartItem = await _context.Cart
           .FirstOrDefaultAsync(m => m.MemberId == memberid);

            if (ModelState.IsValid)
            {
                if (cartItem != null)
                {
                        string sqlQuery = "DELETE dbo.Cart WHERE MemberId = '" + memberid + "'";
                        int affectedRows = _context.Database.ExecuteSqlRaw(sqlQuery);
                }

                TempData["Success"] = "The cart has been cleared!";
                return RedirectToAction(nameof(Index), new { memberid });

            }

            return RedirectToAction(nameof(Index), new { memberid });


        }

        public async Task<IActionResult> CheckOut(string memberid)
        {
            IQueryable<CartItem> cartQuery = _context.Cart;

            if (!string.IsNullOrEmpty(memberid))
            {
                cartQuery = cartQuery.Where(cartItem => cartItem.MemberId == memberid);
            }

            List<CartItem> cartItems = await cartQuery.ToListAsync();

            decimal grandTotal = cartItems.Sum(item => item.Quantity * item.Price);

            var order = new Order
            {
                OrdersId = Guid.NewGuid(),
                MemberId = memberid,
                OrderDate = DateTime.UtcNow,
                TotalAmount = grandTotal,
                OrderItems = cartItems.Select(cartItem => new OrderItem
                {
                    MemberId = memberid,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.ProductName,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    Total = cartItem.Quantity * cartItem.Price,
                    // Set other properties
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //await ClearAsync(memberid); // Call the ClearAsync method
            CartItem cartItem = await _context.Cart
         .FirstOrDefaultAsync(m => m.MemberId == memberid);

            if (ModelState.IsValid)
            {
                if (cartItem != null)
                {
                    string sqlQuery = "DELETE dbo.Cart WHERE MemberId = '" + memberid + "'";
                    int affectedRows = _context.Database.ExecuteSqlRaw(sqlQuery);
                }

                TempData["Success"] = "The cart has been checkout!";
            }

            // Redirect to a confirmation page or payment gateway
            // return RedirectToAction(nameof(Confirmation));

            //return View(); // Render the checkout view
           // return RedirectToAction(nameof(Index), new { memberid });
            return RedirectToAction("CheckOutIndex", "ProductOnline", new { memberid });
        }

    }
}