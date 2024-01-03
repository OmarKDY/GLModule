namespace GLModule.Domain.Entities
{
    public class Cheque
    {
        public int ChequeId { get; set; }
        public decimal Amount { get; set; }
        public bool IsDueCheque { get; set; }
        public DateTime? DueChequeDate { get; set; }
        public int AccountCode { get; set; }
        public virtual Account Account { get; set; }
        public int? DailyJournalId { get; set; }
        public virtual DailyJournal DailyJournal { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
    }
}
