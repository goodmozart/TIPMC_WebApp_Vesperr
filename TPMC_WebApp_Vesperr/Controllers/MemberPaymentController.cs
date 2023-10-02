using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TPMC_WebApp_Vesperr.Data;
using TPMC_WebApp_Vesperr.Models;
using TPMC_WebApp_Vesperr.Models.TPMC;

namespace TPMC_WebApp_Vesperr.Controllers
{
    public class MemberPaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public MemberPaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: MemberPayment
        //public async Task<IActionResult> Index()
        //{
        //      return View(await _context.MemberPayment.ToListAsync());
        //}

        // GET: MemberShare
        public async Task<IActionResult> Index(string id)
        {

            List<MemberPayment> memberPaymentList;

            if (id == null || _context.MemberPayment == null)
            {
                return View();
            }

            // Get the current user from the ClaimsPrincipal
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Check if the current user has the "Administrator" role
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

            if (isAdmin)
            {
                memberPaymentList = await _context.MemberPayment.ToListAsync();
            }
            else
            {
                memberPaymentList = await _context.MemberPayment
                .Where(m => m.FullName == id)
                .ToListAsync();
            }

            if (memberPaymentList.Count == 0)
            {
                return View();
            }

            return View(memberPaymentList);

        }


        // GET: MemberPayment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.MemberPayment == null)
            {
                return NotFound();
            }

            var memberPayment = await _context.MemberPayment
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (memberPayment == null)
            {
                return NotFound();
            }

            return View(memberPayment);
        }

        // GET: MemberPayment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberPayment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,MemberId,EmpNumber,FirstName,LastName,FullName,Department,Section,DatePaid,Principal,Interest,Status,Remarks")] MemberPayment memberPayment)
        {
            if (ModelState.IsValid)
            {
                memberPayment.PaymentId = Guid.NewGuid();
                _context.Add(memberPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberPayment);
        }

        // GET: MemberPayment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.MemberPayment == null)
            {
                return NotFound();
            }

            var memberPayment = await _context.MemberPayment.FindAsync(id);
            if (memberPayment == null)
            {
                return NotFound();
            }
            return View(memberPayment);
        }

        // POST: MemberPayment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PaymentId,MemberId,EmpNumber,FirstName,LastName,FullName,Department,Section,DatePaid,Principal,Interest,Status,Remarks")] MemberPayment memberPayment)
        {
            if (id != memberPayment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberPaymentExists(memberPayment.PaymentId))
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
            return View(memberPayment);
        }

        // GET: MemberPayment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.MemberPayment == null)
            {
                return NotFound();
            }

            var memberPayment = await _context.MemberPayment
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (memberPayment == null)
            {
                return NotFound();
            }

            return View(memberPayment);
        }

        // POST: MemberPayment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.MemberPayment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MemberPayment'  is null.");
            }
            var memberPayment = await _context.MemberPayment.FindAsync(id);
            if (memberPayment != null)
            {
                _context.MemberPayment.Remove(memberPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberPaymentExists(Guid id)
        {
          return _context.MemberPayment.Any(e => e.PaymentId == id);
        }

        // GET: MemberPayment/Create/5
        public async Task<IActionResult> CreateNew(string id)
        {
            if (id == null || _context.MemberPayment == null)
            {
                return NotFound();
            }

            Guid memberGuid = Guid.Parse(id);
            Guid paymentid = Guid.NewGuid();

            var member = _context.Member.FirstOrDefault(m => m.MemberId == memberGuid);

            if (member == null)
            {
                return NotFound();
            }
            else
            {
                string insertSql = @"
                INSERT INTO MemberPayment (PaymentId,MemberId, FirstName, LastName, EmpNumber, FullName, Department, Section)
                VALUES (@PaymentId,@MemberId, @FirstName, @LastName, @EmpNumber, @FullName, @Department, @Section)";

                var parameters = new[]
                {
                new SqlParameter("@PaymentId", paymentid),
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
    }
}
