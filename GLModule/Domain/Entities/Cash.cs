using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLModule.Domain.Entities
{
    [Index(nameof(Cash.CashName), IsUnique = true)]
    public class Cash
    {
        public int CashId { get; set; }
        public string CashName { get; set; }
        public int AccountCode { get; set; }
        [ForeignKey("AccountCode")]
        public virtual Account? Account { get; set; }
    }
}
