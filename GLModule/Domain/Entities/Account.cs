using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GLModule.Domain.Entities
{
    [Index(nameof(Account.AccountName), IsUnique = true)]
    public class Account
    {
        public int AccountCode { get; set; }
        [Required]
        public string AccountName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal InitialBalance { get; set; }
        public bool IsDebit { get; set; }
        public bool IsCredit { get; set; }
        public bool IsParent { get; set; }
        public int? ParentAccountCode { get; set; }
        public virtual Account? ParentAccount { get; set; }
        public int Level { get; set; }
        public virtual Bank? Bank { get; set; }
        public virtual Cash? Cash { get; set; }
        public Guid? AccountTransactionId { get; set; }
        public virtual AccountTransaction? AccountTransaction { get; set; }
        public virtual ICollection<Account>? ChildAccounts { get; set; }
        public virtual ICollection<DailyJournal>? DailyJournals { get; set; }
        public virtual ICollection<CashBankReceipt>? CashBankReceipts { get; set; }
        public virtual ICollection<CashBankReceive>? CashBankReceives { get; set; }
        public virtual ICollection<Cheque>? Cheques { get; set; }
    }

}
