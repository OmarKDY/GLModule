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

namespace GLModule.Controller
{
    [Route("/api/[controller]")]
    [ApiController]
    public class DailyJournalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ExistsHelper _existsHelper;
        private readonly ValidHelper _validHelper;

        public DailyJournalsController(ApplicationDbContext context, ExistsHelper existsHelper, ValidHelper validHelper)
        {
            _context = context;
            _existsHelper = existsHelper;
            _validHelper = validHelper;
        }

        // GET: DailyJournals
        [HttpGet("GetDailyJournalList")]
        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.DailyJournals.Include(d => d.Account).Include(d => d.AccountTransactions).Include(d => d.CashBankReceipt).Include(d => d.CashBankReceive).Include(d => d.Cheque);
            return Ok(await applicationDbContext.ToListAsync());
        }

        // GET: DailyJournals/Details/5
        [HttpGet("GetDailyJournal")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DailyJournals == null)
            {
                return NotFound();
            }

            var dailyJournal = await _context.DailyJournals
                .Include(d => d.Account)
                .Include(d => d.AccountTransactions)
                .Include(d => d.CashBankReceipt)
                .Include(d => d.CashBankReceive)
                .Include(d => d.Cheque)
                .FirstOrDefaultAsync(m => m.DailyJournalId == id);
            if (dailyJournal == null)
            {
                return NotFound();
            }

            return Ok(dailyJournal);
        }

        // GET: DailyJournals/Create
        public IActionResult Create()
        {
            //ViewData["AccountCode"] = new SelectList(_context.Accounts, "AccountCode", "AccountName");
            //ViewData["CashBankReceiptId"] = new SelectList(_context.CashBankReceipts, "CashBankReceiptId", "CashBankReceiptId");
            //ViewData["CashBankReceiveId"] = new SelectList(_context.CashBankReceives, "CashBankReceiveId", "CashBankReceiveId");
            return Ok();
        }

        // POST: DailyJournals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DailyJournalId,Description,Amount,AccountCode,CashBankReceiptId,CashBankReceiveId,ChequeId")] DailyJournal dailyJournal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyJournal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AccountCode"] = new SelectList(_context.Accounts, "AccountCode", "AccountName", dailyJournal.AccountCode);
            //ViewData["CashBankReceiptId"] = new SelectList(_context.CashBankReceipts, "CashBankReceiptId", "CashBankReceiptId", dailyJournal.CashBankReceiptId);
            //ViewData["CashBankReceiveId"] = new SelectList(_context.CashBankReceives, "CashBankReceiveId", "CashBankReceiveId", dailyJournal.CashBankReceiveId);
            return Ok(dailyJournal);
        }

        // GET: DailyJournals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DailyJournals == null)
            {
                return NotFound();
            }

            var dailyJournal = await _context.DailyJournals.FindAsync(id);
            if (dailyJournal == null)
            {
                return NotFound();
            }
            //ViewData["AccountCode"] = new SelectList(_context.Accounts, "AccountCode", "AccountName", dailyJournal.AccountCode);
            //ViewData["CashBankReceiptId"] = new SelectList(_context.CashBankReceipts, "CashBankReceiptId", "CashBankReceiptId", dailyJournal.CashBankReceiptId);
            //ViewData["CashBankReceiveId"] = new SelectList(_context.CashBankReceives, "CashBankReceiveId", "CashBankReceiveId", dailyJournal.CashBankReceiveId);
            return Ok(dailyJournal);
        }

        // POST: DailyJournals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DailyJournalId,Description,Amount,AccountCode,CashBankReceiptId,CashBankReceiveId,ChequeId")] DailyJournal dailyJournal)
        {
            if (id != dailyJournal.DailyJournalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyJournal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyJournalExists(dailyJournal.DailyJournalId))
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
            //ViewData["AccountCode"] = new SelectList(_context.Accounts, "AccountCode", "AccountName", dailyJournal.AccountCode);
            //ViewData["CashBankReceiptId"] = new SelectList(_context.CashBankReceipts, "CashBankReceiptId", "CashBankReceiptId", dailyJournal.CashBankReceiptId);
            //ViewData["CashBankReceiveId"] = new SelectList(_context.CashBankReceives, "CashBankReceiveId", "CashBankReceiveId", dailyJournal.CashBankReceiveId);
            return Ok(dailyJournal);
        }

        // GET: DailyJournals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DailyJournals == null)
            {
                return NotFound();
            }

            var dailyJournal = await _context.DailyJournals
                .Include(d => d.Account)
                .Include(d => d.CashBankReceipt)
                .Include(d => d.CashBankReceive)
                .FirstOrDefaultAsync(m => m.DailyJournalId == id);
            if (dailyJournal == null)
            {
                return NotFound();
            }

            return Ok(dailyJournal);
        }

        // POST: DailyJournals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DailyJournals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DailyJournals'  is null.");
            }
            var dailyJournal = await _context.DailyJournals.FindAsync(id);
            if (dailyJournal != null)
            {
                _context.DailyJournals.Remove(dailyJournal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyJournalExists(int id)
        {
          return (_context.DailyJournals?.Any(e => e.DailyJournalId == id)).GetValueOrDefault();
        }
    }
}
