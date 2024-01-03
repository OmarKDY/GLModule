using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GLModule.Data;
using GLModule.Domain.Entities;
using GLModule.Infrastructure.Helpers;
using System.Security.Policy;

namespace GLModule.Controller
{
    [Route("/api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ExistsHelper _existsHelper;

        public BanksController(ApplicationDbContext context, ExistsHelper existsHelper)
        {
            _context = context;
            _existsHelper = existsHelper;
        }

        // GET: Banks
        [HttpGet("GetBankList")]
        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.Banks.Include(c => c.Account);
            return Ok(await applicationDbContext.ToListAsync());
        }

        // GET: Banks/Details/5
        [HttpGet("GetBank/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Banks == null)
            {
                return NotFound(new { success = false, message = "There's no bank with this specific id" });
            }

            var bank = await _context.Banks
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.BankId == id);
            if (bank == null)
            {
                return NotFound(new { success = false, message = "There's no bank with this specific id" });
            }
            return Ok(bank);
        }

        // POST: Banks/Create
        [HttpPost("AddBank")]
        public async Task<IActionResult> Create([Bind("BankId,BankName,AccountCode")]Bank bank)
        {
            if (!_existsHelper.RowExists<Bank>(bank.BankId,"Banks"))
            {
                if (!_existsHelper.AccountTransactionExists(bank.AccountCode))
                {
                    if (ModelState.IsValid)
                    {
                        bank.Account = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountCode == bank.AccountCode);
                        if (bank.Account.IsParent == false)
                        {
                            var existingBank = await _context.Banks.FirstOrDefaultAsync(b => b.AccountCode == bank.AccountCode);
                            if (existingBank == null)
                            {
                                _context.Add(bank);
                                await _context.SaveChangesAsync();
                                return Ok(new { success = true, message = $"{bank.BankName} added successfully" });
                            }
                            return Ok(new { success = false, message = $"Account already added for {existingBank.BankName} before" });
                        }
                        return BadRequest(new { success = false, message = $"You can't add a parent account to the {bank.BankName}" });
                    }
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return BadRequest(new { success = false, errors });
                }
                return BadRequest(new { success = false, message = $"There're transactions has been made with {bank.BankName}" });
            }
            return BadRequest(new { success = false, message = "There's a bank added before with this specific id" });
        }

        // POST: Banks/Edit/5
        [HttpPost("EditBank/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("BankId,BankName,AccountCode")] Bank bank)
        {
            if (id != bank.BankId)
            {
                return NotFound(new { success = false, message = $"The account code {id} not equal the new account code {bank.BankId}" });
            }
            if (!_existsHelper.AccountTransactionExists(bank.AccountCode))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(bank);
                        await _context.SaveChangesAsync();
                        return Ok(new { success = true, message = $"{bank.BankName} modified successfully" });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_existsHelper.RowExists<Bank>(bank.BankId, "Banks"))
                        {
                            return NotFound(new { success = false, message = "There's no bank with this specific id" });
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
            return BadRequest(new { success = false, message = $"There're transactions has been made with {bank.BankName}" });
        }

        // POST: Banks/Delete/5
        [HttpPost("DeleteBank/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Banks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Banks'  is null.");
            }
            var bank = await _context.Banks.FindAsync(id);
            if (bank != null)
            {
                if (!_existsHelper.AccountTransactionExists(bank.AccountCode))
                {
                    _context.Banks.Remove(bank);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Bank deleted successfully" });
                }
                return BadRequest(new { success = false, message = $"There're transactions has been made with {bank.BankName}" });
            }
            return BadRequest(new { success = false, message = "There's no Bank with this specific id" });
        }
    }
}
