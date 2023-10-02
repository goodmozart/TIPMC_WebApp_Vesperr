using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPMC_WebApp_Vesperr.Data;
using TPMC_WebApp_Vesperr.Models.POS;

namespace TPMC_WebApp_Vesperr.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMember()
        {
            return await _context.Member.ToListAsync();
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(Guid id)
        {
            var Member = await _context.Member.FindAsync(id);

            if (Member == null)
            {
                return NotFound();
            }

            return Member;
        }

        // PUT: api/Member/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(Guid id, Member Member)
        {
            if (id != Member.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(Member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Member
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member Member)
        {
            _context.Member.Add(Member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = Member.MemberId }, Member);
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Member>> DeleteMember(Guid id)
        {
            var Member = await _context.Member.FindAsync(id);
            if (Member == null)
            {
                return NotFound();
            }

            _context.Member.Remove(Member);
            await _context.SaveChangesAsync();

            return Member;
        }

        private bool MemberExists(Guid id)
        {
            return _context.Member.Any(e => e.MemberId == id);
        }
    }
}
