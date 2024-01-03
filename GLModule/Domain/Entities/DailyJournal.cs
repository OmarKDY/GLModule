namespace GLModule.Domain.Entities
{
    public class DailyJournal
    {
        public int DailyJournalId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int AccountCode { get; set; }
        public virtual Account Account { get; set; }
        public int? CashBankReceiptId { get; set; }
        public virtual CashBankReceipt CashBankReceipt { get; set; }
        public int? CashBankReceiveId { get; set; }
        public virtual CashBankReceive CashBankReceive { get; set; }
        public int? ChequeId { get; set; }
        public virtual Cheque Cheque { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
    }
}
