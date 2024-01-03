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
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ExistsHelper _existsHelper;
        private readonly ValidHelper _validHelper;

        public AccountsController(ApplicationDbContext context, ExistsHelper existsHelper, ValidHelper validHelper)
        {
            _context = context;
            _existsHelper = existsHelper;
            _validHelper = validHelper;
        }

        // GET: Accounts
        [HttpGet("GetAccountList")]
        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.Accounts.Include(a => a.ChildAccounts);
            return Ok(await applicationDbContext.ToListAsync());
        }

        // GET: ChildAccounts
        [HttpGet("GetChildAccountList/{accountCode}")]
        public async Task<IActionResult> ChildAccountList(int accountCode)
        {
            var parentWithChildAccounts = await _context.Accounts.Include(a => a.ChildAccounts).FirstOrDefaultAsync(a => a.AccountCode == accountCode);
            if (parentWithChildAccounts == null)
            {
                return NotFound(new { success = false, message = $"No account found with the account Code {accountCode}." });
            }

            var childAccounts = parentWithChildAccounts.ChildAccounts;

            return Ok(new { success = true, parentWithChildAccounts });
        }

        // GET: AccountTransactions
        [HttpGet("GetAccountTransactions/{accountCode}")]
        public async Task<IActionResult> AccountTransactions(int accountCode)
        {
            var account = await _context.Accounts.Include(ca => ca.ChildAccounts).Include(at => at.AccountTransaction).FirstOrDefaultAsync(a => a.AccountCode == accountCode);
            if (account != null)
            {
                if (!account.IsParent)
                {
                    return Ok(new { success = true, account });
                }
                else
                {
                    return Ok(new { success = true, account, account.ChildAccounts });
                }
            }
            return NotFound(new { success = false, message = $"No account found with the account Code {accountCode}." });
        }

        // GET: Accounts/Details/5
        [HttpGet("GetAccount/{accountCode}")]
        public async Task<IActionResult> Details(int? accountCode)
        {
            if (accountCode == null || _context.Accounts == null)
            {
                return NotFound(new { success = false, message = "There's no account with this specific id" });
            }

            var account = await _context.Accounts
                .Include(a => a.ChildAccounts)
                .FirstOrDefaultAsync(m => m.AccountCode == accountCode);

            if (account == null)
            {
                return NotFound(new { success = false, message = "There's no account with this specific id" });
            }

            return Ok(account);
        }

        [HttpPost("AddAccount")]
        public async Task<IActionResult> Create([Bind("AccountCode,AccountName,Description,InitialBalance,IsDebit,IsCredit,IsParent,ParentAccountCode,Level,AccountTransactionId")] Account account)
        {
            if (!_existsHelper.RowExists<Account>(account.AccountCode, "Accounts"))
            {
                if (!_existsHelper.AccountTransactionExists(account.AccountCode))
                {
                    if (ModelState.IsValid)
                    {
                        if (_validHelper.IsValidParentAccount(account) && _validHelper.IsValidInitialBalance(account))
                        {
                            Guid accountTransactionGuid = Guid.NewGuid();
                            var accountTransaction = new AccountTransaction
                            {
                                AccountTransactionId = accountTransactionGuid,
                                DocType = 20,
                                TotalAmount = account.InitialBalance,
                                PaymentType = "InitialBalance",
                                IsInitialBalance = account.InitialBalance != 0,
                                AccountCode = account.AccountCode,
                                Description = account.Description
                            };
                            accountTransaction.DocSerial = $"{accountTransaction.DocType}-{account.AccountCode}";
                            account.AccountTransaction = accountTransaction;
                            if (account.ParentAccountCode != null || account.ParentAccountCode != 0)
                            {
                                account.AccountCode = (int)(account.ParentAccountCode + account.AccountCode);
                            }
                            _context.Add(account);
                            await _context.SaveChangesAsync();
                            return Ok(new { success = true, message = $"Account {account.AccountName} added successfully" });
                        }
                        else
                        {
                            return BadRequest(new { success = false, message = "Invalid parent account or initial balance" });
                        }
                    }
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return BadRequest(new { success = false, errors });
                }
                return BadRequest(new { success = false, message = $"There're transactions has been made with this account {account.AccountName}" });
            }
            return BadRequest(new { success = false, message = "There's an account added before with this specific account code" });
        }

        // POST: Accounts/Edit/5
        [HttpPost("EditAccount/{accountCode}")]
        public async Task<IActionResult> Edit(int accountCode, [Bind("AccountCode,AccountName,Description,InitialBalance,IsDebit,IsCredit,IsParent,ParentAccountCode,Level,AccountTransactionId")] Account account)
        {
            if (accountCode != account.AccountCode)
            {
                return NotFound(new { success = false, message = $"The account code {accountCode} does not match the new account code {account.AccountCode}" });
            }

            if (!_existsHelper.AccountTransactionExists(account.AccountCode))
            {
                if (ModelState.IsValid)
                {
                    if (_validHelper.IsValidParentAccount(account) && _validHelper.IsValidInitialBalance(account))
                    {
                        try
                        {
                            account.AccountTransaction = await _existsHelper.GetAccountTransaction(account.AccountTransactionId);
                            if (account.AccountTransaction != null)
                            {
                                account.AccountTransaction.DocSerial = $"{account.AccountTransaction.DocType}-{account.AccountCode}";
                            }
                            if (account.ParentAccountCode != null || account.ParentAccountCode != 0)
                            {
                                account.AccountCode = (int)(account.ParentAccountCode + account.AccountCode);
                            }
                            _context.Update(account);
                            await _context.SaveChangesAsync();
                            return Ok(new { success = true, message = $"Account {account.AccountName} modified successfully" });
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!_existsHelper.RowExists<Account>(account.AccountCode, "Accounts"))
                            {
                                return NotFound(new { success = false, message = $"There's no account with this specific account code" });
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Invalid parent account or initial balance" });
                    }
                }
            }
            return BadRequest(new { success = false, message = $"There're transactions that have been made with this account {account.AccountName}" });
        }

        // POST: Accounts/Delete/5
        [HttpPost("DeleteAccount/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                if (!_existsHelper.AccountTransactionExists(account.AccountCode))
                {
                    _context.Accounts.Remove(account);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = $"Account {account.AccountName} deleted successfully" });
                }
                return BadRequest(new { success = false, message = $"There're transactions has been made with this specific Account {account.AccountName}" });
            }
            return BadRequest(new { success = false, message = "There's no Account with this specific Code" });
        }
    }
}
