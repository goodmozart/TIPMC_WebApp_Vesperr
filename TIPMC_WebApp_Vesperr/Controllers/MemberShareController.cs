using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models;
using TIPMC_WebApp_Vesperr.Models.Online;
using TIPMC_WebApp_Vesperr.Models.POS;
using TIPMC_WebApp_Vesperr.Models.TIPMC;

namespace TIPMC_WebApp_Vesperr.Controllers
{
    public class MemberShareController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public MemberShareController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Error(string error)
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            ViewBag.ErrorMessage = error;
            return View();

        }

        // GET: MemberShare
        public async Task<IActionResult> Index(string id)
        {
            List<MemberShares> memberSharesList;

            if (id == null || _context.MemberShares == null)
            {
                return View();
            }


            // Get the current user from the ClaimsPrincipal
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Check if the current user has the "Administrator" role
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

            if (isAdmin)
            {
                memberSharesList = await _context.MemberShares.ToListAsync();
            }
            else
            {
                memberSharesList = await _context.MemberShares
                .Where(m => m.FullName == id)
                .ToListAsync();
            }

            if (memberSharesList.Count == 0)
            {
                return View();
            }

            return View(memberSharesList);

        }

        // GET: MemberShare/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.MemberShares == null)
            {
                return NotFound();
            }

            var memberShares = await _context.MemberShares
                .FirstOrDefaultAsync(m => m.ShareId == id);
            
            if (memberShares == null)
            {
                return NotFound();
            }

            return View(memberShares);
        }

        // GET: MemberShare/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberShare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShareId,MemberId,EmpNumber,FirstName,LastName,FullName,Department,Section,DatePosted,NumberShare,Amount,Remarks,UpdateDate,Status")] MemberShares memberShares)
        {
            if (ModelState.IsValid)
            {
                memberShares.ShareId = Guid.NewGuid();
                _context.Add(memberShares);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberShares);
        }

        // GET: MemberShare/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.MemberShares == null)
            {
                return NotFound();
            }

            var memberShares = await _context.MemberShares.FindAsync(id);
            if (memberShares == null)
            {
                return NotFound();
            }
            return View(memberShares);
        }

        // POST: MemberShare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ShareId,MemberId,EmpNumber,FirstName,LastName,FullName,Department,Section,DatePosted,NumberShare,Amount,Remarks,UpdateDate,Status")] MemberShares memberShares)
        {
            if (id != memberShares.ShareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberShares);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberSharesExists(memberShares.ShareId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var member = _context.MemberShares.FirstOrDefault(m => m.ShareId == id);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), new { memberid = member });
            }
            return View(memberShares);
        }

        // GET: MemberShare/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.MemberShares == null)
            {
                return NotFound();
            }

            var memberShares = await _context.MemberShares
                .FirstOrDefaultAsync(m => m.ShareId == id);
            if (memberShares == null)
            {
                return NotFound();
            }

            return View(memberShares);
        }

        // POST: MemberShare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.MemberShares == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MemberShares'  is null.");
            }
            var memberShares = await _context.MemberShares.FindAsync(id);
            if (memberShares != null)
            {
                _context.MemberShares.Remove(memberShares);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberSharesExists(Guid id)
        {
          return _context.MemberShares.Any(e => e.ShareId == id);
        }

        // GET: MemberShare/Create/5
        public async Task<IActionResult> CreateNew(string id)
        {
            if (id == null || _context.MemberShares == null)
            {
                return NotFound();
            }

            Guid memberGuid = Guid.Parse(id);
            Guid shareid = Guid.NewGuid();

            var member = _context.Member.FirstOrDefault(m => m.MemberId == memberGuid);

            if (member == null)
            {
                return NotFound();
            }
            else
            {
                string insertSql = @"
                INSERT INTO MemberShares (ShareId,MemberId, FirstName, LastName, EmpNumber, FullName, Department, Section)
                VALUES (@ShareId,@MemberId, @FirstName, @LastName, @EmpNumber, @FullName, @Department, @Section)";

                var parameters = new[]
                {
                new SqlParameter("@ShareId", shareid),
                    new SqlParameter("@MemberId", memberGuid),
                new SqlParameter("@FirstName", member.FirstName != null ?(object) member.FirstName : DBNull.Value),
                new SqlParameter("@LastName", member.LastName != null ?(object) member.LastName : DBNull.Value),
                new SqlParameter("@EmpNumber", member.EmpNumber != null ?(object) member.EmpNumber : DBNull.Value),
                new SqlParameter("@FullName", member.FirstName + " " + member.LastName),
                new SqlParameter("@Department", member.Department != null ? (object)member.Department : DBNull.Value),
                new SqlParameter("@Section", member.Section != null ? (object)member.Section : DBNull.Value   )
        };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);


                // Redirect to the "Edit" action, passing the newly inserted MemberShareNew ID
                //return RedirectToAction("DisplayByEmpNo", new { empno = member.EmpNumber });
                return RedirectToAction(nameof(Index), new { memberid = id });
            }
        }



        public async Task<IActionResult> IndexMember(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(await _context.Member.ToListAsync());
        }
    }
}
