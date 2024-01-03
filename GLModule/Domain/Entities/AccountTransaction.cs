using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GLModule.Domain.Entities
{
    [Index(nameof(AccountTransaction.DocSerial), IsUnique = true)]
    public class AccountTransaction
    {
        [Key]
        public Guid AccountTransactionId { get; set; }
        public int DocType { get; set; }
        public string? DocSerial { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsInitialBalance { get; set; }
        [Required]
        public int AccountCode { get; set; }
        public virtual Account? Account { get; set; }
        public int? CashBankReceiptId { get; set; }
        public virtual CashBankReceipt? CashBankReceipt { get; set; }
        public int? CashBankReceiveId { get; set; }
        public virtual CashBankReceive? CashBankReceive { get; set; }
        public int? ChequeId { get; set; }
        public virtual Cheque? Cheque { get; set; }
        public int? DailyJournalId { get; set; }
        public virtual DailyJournal? DailyJournal { get; set; }
        public bool IsDailyJournal { get; set; }
    }
}
