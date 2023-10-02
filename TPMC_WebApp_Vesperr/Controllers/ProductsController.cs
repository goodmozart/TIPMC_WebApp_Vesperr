using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models.Online;
using TIPMC_WebApp_Vesperr.Models.POS;

namespace TIPMC_WebApp_Vesperr.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productOnline = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productOnline == null)
            {
                return NotFound();
            }

            return View(productOnline);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile ImageUpload, [Bind("Id,Name,Slug,Description,Price,CategoryId,Image")] ProductOnline productOnline)
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null)
                {
                    // Handle the uploaded image
                    if (ImageUpload.Length > 0)
                    {
                        // Getting FileName
                        var fileName = Path.GetFileName(ImageUpload.FileName);

                        //// Assigning Unique Filename (Guid)
                        //var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        // Assigning Unique Filename (Guid)
                        var myUniqueFileName = productOnline.Slug;

                        // Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        // Concatenating FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        // Combining two strings into a path.
                        var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media", "products")).Root + $@"\{newFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            ImageUpload.CopyTo(fs);
                            fs.Flush();
                        }

                        // Update the 'Image' property with the filename
                        productOnline.Image = newFileName;
                    }
                }

                _context.Add(productOnline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productOnline.CategoryId);
            
            return View(productOnline);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productOnline = await _context.Products.FindAsync(id);
            if (productOnline == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productOnline.CategoryId);
            return View(productOnline);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Slug,Description,Price,CategoryId,Image")] ProductOnline productOnline)
        {
            if (id != productOnline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productOnline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductOnlineExists(productOnline.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productOnline.CategoryId);
            return View(productOnline);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productOnline = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productOnline == null)
            {
                return NotFound();
            }

            // Delete the image file from the server
            if (!string.IsNullOrEmpty(productOnline.Image))
            {
                var imagePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media", "products"), productOnline.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            return View(productOnline);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var productOnline = await _context.Products.FindAsync(id);
            if (productOnline != null)
            {
                _context.Products.Remove(productOnline);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductOnlineExists(long id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
