namespace GLModule.Domain.Entities
{
    public class CashBankReceive
    {
        public int CashBankReceiveId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int AccountCode { get; set; }
        public virtual Account Account { get; set; }
        public int? DailyJournalId { get; set; }
        public virtual DailyJournal DailyJournal { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
    }
}
