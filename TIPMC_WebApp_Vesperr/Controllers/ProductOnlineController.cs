using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models;
using TIPMC_WebApp_Vesperr.Models.Online;

namespace TIPMC_WebApp_Vesperr.Controllers
{
    public class ProductOnlineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProductOnlineController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string categorySlug = "", int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

                return View(await _context.Products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
            }

            Category category = await _context.Categories.Where(c => c.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");

            // Get the list of categories for the dropdown
            List<Category> categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Slug", "Name", categorySlug);


            var productsByCategory = _context.Products.Where(p => p.CategoryId == category.Id);
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsByCategory.Count() / pageSize);

            return View(await productsByCategory.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        private bool ProductOnlineExists(long id)
        {
          return _context.Products.Any(e => e.Id == id);
        }


        public async Task<IActionResult> CheckOutIndex(string memberid)
        {


            // Get the current user from the ClaimsPrincipal
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Check if the current user has the "Administrator" role
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

            if (isAdmin)
            {
                // Retrieve the Order based on memberid
                Order order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync();
                if (order != null)
                    {
                        return View(order);
                    }
            }
            else
            {
                if (!string.IsNullOrEmpty(memberid))
                {
                    // Retrieve the Order based on memberid
                    Order order = await _context.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.MemberId == memberid);

                    if (order != null)
                    {
                        return View(order);
                    }
                }
            }

            //if (!string.IsNullOrEmpty(memberid))
            //{
            //    // Retrieve the Order based on memberid
            //    Order order = await _context.Orders
            //        .Include(o => o.OrderItems)
            //        .FirstOrDefaultAsync(o => o.MemberId == memberid);

            //    if (order != null)
            //    {
            //        return View(order);
            //    }
            //}

            // Handle case where memberid is null or order is not found
            return View(); // Return an empty view or appropriate error handling
        }

    }
}
