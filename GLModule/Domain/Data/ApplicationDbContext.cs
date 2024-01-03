using Microsoft.EntityFrameworkCore;
using GLModule.Domain.Entities;
using GLModule.Data;

namespace GLModule.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasIndex(e => e.AccountName)
                .IsUnique();
            modelBuilder.Entity<Cash>()
                .HasIndex(e => e.CashName)
                .IsUnique();
            modelBuilder.Entity<Bank>()
                .HasIndex(e => e.BankName)
                .IsUnique();
            modelBuilder.Entity<AccountTransaction>()
                .HasIndex(e => e.DocSerial)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountCode);

            modelBuilder.Entity<Account>()
                .Property(a => a.AccountCode)
                .ValueGeneratedNever();

            modelBuilder.Entity<Account>()
                .HasMany(a => a.ChildAccounts)
                .WithOne(a => a.ParentAccount)
                .HasForeignKey(a => a.ParentAccountCode);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Bank)
                .WithOne(b => b.Account)
                .HasForeignKey<Bank>(b => b.AccountCode);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Cash)
                .WithOne(c => c.Account)
                .HasForeignKey<Cash>(c => c.AccountCode);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.DailyJournals)
                .WithOne(d => d.Account)
                .HasForeignKey(d => d.AccountCode);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.CashBankReceipts)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.AccountCode);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.CashBankReceives)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.AccountCode);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Cheques)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountCode);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.AccountTransaction)
                .WithOne(at => at.Account);

            modelBuilder.Entity<DailyJournal>()
                .HasOne(d => d.CashBankReceipt)
                .WithOne(r => r.DailyJournal)
                .HasForeignKey<DailyJournal>(d => d.CashBankReceiptId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DailyJournal>()
                .HasOne(d => d.CashBankReceive)
                .WithOne(r => r.DailyJournal)
                .HasForeignKey<DailyJournal>(d => d.CashBankReceiveId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DailyJournal>()
                .HasOne(d => d.Cheque)
                .WithOne(c => c.DailyJournal)
                .HasForeignKey<Cheque>(c => c.DailyJournalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DailyJournal>()
                .HasOne(d => d.Account)
                .WithMany(a => a.DailyJournals)
                .HasForeignKey(d => d.AccountCode);

            modelBuilder.Entity<CashBankReceipt>()
                .HasOne(r => r.Account)
                .WithMany(a => a.CashBankReceipts)
                .HasForeignKey(r => r.AccountCode);


            modelBuilder.Entity<CashBankReceive>()
                .HasOne(r => r.Account)
                .WithMany(a => a.CashBankReceives)
                .HasForeignKey(r => r.AccountCode);


            modelBuilder.Entity<Cheque>()
                .HasOne(c => c.Account)
                .WithMany(a => a.Cheques)
                .HasForeignKey(c => c.AccountCode);

            modelBuilder.Entity<Bank>()
                .HasOne(b => b.Account)
                .WithOne(a => a.Bank)
                .HasForeignKey<Bank>(b => b.AccountCode);

            modelBuilder.Entity<Cash>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Cash)
                .HasForeignKey<Cash>(c => c.AccountCode);

            modelBuilder.Entity<AccountTransaction>()
                .HasKey(at => at.AccountTransactionId);

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(at => at.Account)
                .WithOne(a => a.AccountTransaction);

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(at => at.CashBankReceipt)
                .WithMany(r => r.AccountTransactions)
                .HasForeignKey(at => at.CashBankReceiptId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(at => at.CashBankReceive)
                .WithMany(r => r.AccountTransactions)
                .HasForeignKey(at => at.CashBankReceiveId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(at => at.Cheque)
                .WithMany(c => c.AccountTransactions)
                .HasForeignKey(at => at.ChequeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(at => at.DailyJournal)
                .WithMany(d => d.AccountTransactions)
                .HasForeignKey(at => at.DailyJournalId)
                .OnDelete(DeleteBehavior.Restrict);
        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<DailyJournal> DailyJournals { get; set; }
        public DbSet<CashBankReceipt> CashBankReceipts { get; set; }
        public DbSet<CashBankReceive> CashBankReceives { get; set; }
        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Cash> Cashes { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
    }
}