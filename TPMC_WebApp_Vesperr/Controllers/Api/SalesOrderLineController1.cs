using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models.POS;


namespace TIPMC_WebApp_Vesperr.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderLineController1 : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly Services.POS.IRepository _pos;


        public SalesOrderLineController1(ApplicationDbContext context, IHttpContextAccessor httpContext, Services.POS.IRepository pos)
        {
            _context = context;
            _httpContext = httpContext;
            _pos = pos;

        }

        // GET: api/SalesOrderLine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrderLine>>> GetSalesOrderLine()
        {
            return await _context.SalesOrderLine.ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLines()
        {
            List<SalesOrderLine> lines = new List<SalesOrderLine>();
            try
            {
                var paramGuidString = _httpContext.HttpContext.Request.Query["SalesOrderId"].ToString();
                Guid SalesOrderId = new Guid(paramGuidString);
                lines = await _context.SalesOrderLine.Include(x => x.Product).Where(x => x.SalesOrderId.Equals(SalesOrderId)).ToListAsync();
                return Ok(new { lines });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { error = ex.Message });
            }

        }

        // GET: api/SalesOrderLine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderLine>> GetSalesOrderLine(Guid id)
        {
            var SalesOrderLine = await _context.SalesOrderLine.FindAsync(id);

            if (SalesOrderLine == null)
            {
                return NotFound();
            }

            return SalesOrderLine;
        }

        // PUT: api/SalesOrderLine/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrderLine(Guid id, SalesOrderLine SalesOrderLine)
        {
            if (id != SalesOrderLine.SalesOrderLineId)
            {
                return BadRequest();
            }

            _context.Entry(SalesOrderLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderLineExists(id))
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

        // POST: api/SalesOrderLine
        [HttpPost]
        public async Task<ActionResult<SalesOrderLine>> PostSalesOrderLine(SalesOrderLine SalesOrderLine)
        {
            _context.SalesOrderLine.Add(SalesOrderLine);

            //SalesOrder salesOrder = _context.SalesOrder.Where(x => x.SalesOrderId.Equals(SalesOrderLine.SalesOrderId)).FirstOrDefault();

            InvenTran tran = new InvenTran();
            tran.InvenTranId = Guid.NewGuid();
            tran.Number = _pos.GenerateInvenTranNumber();
            tran.ProductId = SalesOrderLine.ProductId;
            tran.TranSourceId = SalesOrderLine.SalesOrderLineId;
            tran.TranSourceNumber = SalesOrderLine.Number;
            tran.TranSourceType = "SO";
            tran.Quantity = SalesOrderLine.Quantity * -1; //minus for inventory deduction
            tran.InvenTranDate = DateTime.Now;
            _context.InvenTran.Add(tran);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesOrderLine", new { id = SalesOrderLine.SalesOrderLineId }, SalesOrderLine);
        }

        // DELETE: api/SalesOrderLine/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SalesOrderLine>> DeleteSalesOrderLine(Guid id)
        {
            var SalesOrderLine = await _context.SalesOrderLine.FindAsync(id);
            if (SalesOrderLine == null)
            {
                return NotFound();
            }

            _context.SalesOrderLine.Remove(SalesOrderLine);
            await _context.SaveChangesAsync();

            return SalesOrderLine;
        }

        private bool SalesOrderLineExists(Guid id)
        {
            return _context.SalesOrderLine.Any(e => e.SalesOrderLineId == id);
        }
    }
}
