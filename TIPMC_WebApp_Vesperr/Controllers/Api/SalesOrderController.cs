﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models.POS;

namespace TIPMC_WebApp_Vesperr.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Services.POS.IRepository _pos;

        public SalesOrderController(ApplicationDbContext context, Services.POS.IRepository pos)
        {
            _context = context;
            _pos = pos;
        }

        // GET: api/SalesOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrder>>> GetSalesOrder()
        {
            return await _context.SalesOrder.ToListAsync();
        }

        // GET: api/SalesOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrder>> GetSalesOrder(Guid id)
        {
            var salesOrder = await _context.SalesOrder.FindAsync(id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            return salesOrder;
        }

        // PUT: api/SalesOrder/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrder(Guid id, SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderId)
            {
                return BadRequest();
            }

            _context.Entry(salesOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderExists(id))
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

        //POST: api/SalesOrder/InitiateNewPOSTrans
        [HttpPost("[action]")]
        public async Task<IActionResult> InitiateNewPOSTrans([FromBody] JsonObject salesMemberJson)
        {
            var memberIdValue = salesMemberJson["MemberId"];
            Guid memberId = new Guid(memberIdValue.ToString());
            
            SalesOrder salesOrder = new SalesOrder();
            salesOrder.Number = _pos.GenerateSONumber();
            salesOrder.SalesOrderDate = DateTime.Now;
            
            //random customer
            //Member salesMember = new Member();
            //salesMember = await _context.Member.FirstOrDefaultAsync();
            
            if (memberIdValue != null)
            {
                //random customer
                //salesOrder.MemberId = salesMember.MemberId;
                salesOrder.MemberId = memberId;
            }

            _context.SalesOrder.Add(salesOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesOrder", new { id = salesOrder.SalesOrderId }, salesOrder);
        }

        // POST: api/SalesOrder
        [HttpPost]
        public async Task<ActionResult<SalesOrder>> PostSalesOrder(SalesOrder salesOrder)
        {
            _context.SalesOrder.Add(salesOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesOrder", new { id = salesOrder.SalesOrderId }, salesOrder);
        }

        // DELETE: api/SalesOrder/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SalesOrder>> DeleteSalesOrder(Guid id)
        {
            var salesOrder = await _context.SalesOrder.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            _context.SalesOrder.Remove(salesOrder);
            await _context.SaveChangesAsync();

            return salesOrder;
        }

        private bool SalesOrderExists(Guid id)
        {
            return _context.SalesOrder.Any(e => e.SalesOrderId == id);
        }
    }
}
