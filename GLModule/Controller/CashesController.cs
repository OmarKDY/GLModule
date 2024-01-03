using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GLModule.Data;
using GLModule.Infrastructure.Helpers;
using GLModule.Domain.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GLModule.Controller
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CashesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ExistsHelper _existsHelper;

        public CashesController(ApplicationDbContext context, ExistsHelper existsHelper)
        {
            _context = context;
            _existsHelper = existsHelper;
        }

        // GET: Cashes
        [HttpGet("GetCashList")]
        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.Cashes.Include(c => c.Account);
            return Ok(await applicationDbContext.ToListAsync());
        }

        // GET: Cashes/Details/5
        [HttpGet("GetCash")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cashes == null)
            {
                return NotFound(new { success = false, message = "There's no cash with this specific id" });
            }

            var cash = await _context.Cashes
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.CashId == id);
            if (cash == null)
            {
                return NotFound(new { success = false, message = "There's no cash with this specific id" });
            }
            return Ok(cash);
        }

        [HttpPost("AddCash")]
        public async Task<IActionResult> Create([Bind("CashId,CashName,AccountCode")] Cash cash)
        {
            if (!_existsHelper.RowExists<Cash>(cash.CashId, "Cashes"))
            {
                if (!_existsHelper.AccountTransactionExists(cash.AccountCode))
                {
                    if (ModelState.IsValid)
                    {
                        cash.Account = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountCode == cash.AccountCode);
                        if (cash.Account.IsParent == false)
                        {
                            var existingCash = await _context.Cashes.FirstOrDefaultAsync(b => b.AccountCode == cash.AccountCode);
                            if (existingCash == null)
                            {
                                _context.Add(cash);
                                await _context.SaveChangesAsync();
                                return Ok(new { success = true, message = $"{cash.CashName} added successfully" });
                            }
                            return Ok(new { success = false, message = $"Account already added for {existingCash.CashName} before" });
                        }
                        return BadRequest(new { success = false, message = $"You can't add a parent account to the {cash.CashName}" });
                    }
                    //var account  = new SelectList(_context.Accounts, "AccountCode", "AccountName", cash.AccountCode);
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return BadRequest(new { success = false, errors });
                }
                return BadRequest(new { success = false, message = $"There're transactions has been made with the {cash.CashName}" });
            }
            return BadRequest(new { success = false, message = "There's a cash added before with this specific id" });
        }

        [HttpPost("EditCash/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("CashId,CashName,AccountCode")] Cash cash)
        {
            if (id != cash.CashId)
            {
                return NotFound(new { success = false, message = $"The account code {id} not equal the new account code {cash.CashId}" });
            }
            if (!_existsHelper.AccountTransactionExists(cash.AccountCode))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(cash);
                        await _context.SaveChangesAsync();
                        return Ok(new { success = true, message = "Cash modified successfully" });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_existsHelper.RowExists<Cash>(cash.CashId, "Cashes"))
                        {
                            return NotFound(new { success = false, message = "There's no cash with this specific id" });
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                //var account  = new SelectList(_context.Accounts, "AccountCode", "AccountName", cash.AccountCode);
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, errors });
            }
            return BadRequest(new { success = false, message = $"There're transactions has been made with {cash.CashName}" });
        }

        // POST: Cashes/Delete/5
        [HttpPost("DeleteCash/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Cashes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cashes'  is null.");
            }
            var cash = await _context.Cashes.FindAsync(id);
            if (cash != null)
            {
                if (!_existsHelper.AccountTransactionExists(cash.AccountCode))
                {
                    _context.Cashes.Remove(cash);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Cash deleted successfully" });
                }
                return BadRequest(new { success = false, message = $"There're transactions has been made with {cash.CashName}" });
            }
            return BadRequest(new { success = false, message = "There's no cash with this specific id" });
        }

    }
}
