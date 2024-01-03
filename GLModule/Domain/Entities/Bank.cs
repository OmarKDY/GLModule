using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLModule.Domain.Entities
{
    [Index(nameof(Bank.BankName), IsUnique = true)]
    public class Bank
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public int AccountCode { get; set; }
        [ForeignKey("AccountCode")]
        public virtual Account? Account { get; set; }
    }
}
