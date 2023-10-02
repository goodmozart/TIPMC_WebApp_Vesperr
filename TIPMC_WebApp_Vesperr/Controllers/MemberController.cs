using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models.POS;

namespace TIPMC_WebApp_Vesperr.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Member
        public async Task<IActionResult> Index()
        {
              return View(await _context.Member.ToListAsync());
        }

        public async Task<IActionResult> IndexMember(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(await _context.Member.ToListAsync());
        }

        public async Task<IActionResult> IndexPayment(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(await _context.Member.ToListAsync());
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,EmpNumber,FirstName,LastName,Name,Description,Phone,Email,Address,Address2,Department,Section,Designation,CreditLimits,AccessLevel,DateJoined")] Member member)
        {
            if (ModelState.IsValid)
            {
                member.MemberId = Guid.NewGuid();
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // GET: Member/Edit/5
        public ActionResult EditNew(string email)
        {

            if (email == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = _context.Member.FirstOrDefault(m => m.Email == email);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MemberId,EmpNumber,FirstName,LastName,Name,Description,Phone,Email,Address,Address2,Department,Section,Designation,CreditLimits,AccessLevel,DateJoined")] Member member)
        {
            if (id != member.MemberId)
            {
                Guid editID = member.MemberId;
                
                if (editID != member.MemberId)
                {
                    return NotFound();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(member);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!MemberExists(member.MemberId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        //return RedirectToAction(nameof(Index));
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            return View(member);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Member == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Member'  is null.");
            }
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(Guid id)
        {
          return _context.Member.Any(e => e.MemberId == id);
        }


    }
}
