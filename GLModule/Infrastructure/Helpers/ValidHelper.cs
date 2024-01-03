using GLModule.Data;
using GLModule.Domain.Entities;

namespace GLModule.Infrastructure.Helpers
{
    public class ValidHelper
    {
        private readonly ApplicationDbContext _context;

        public ValidHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        internal bool IsValidParentAccount(Account account)
        {
            return !(account.ParentAccountCode != null && !account.ParentAccount.IsParent);
        }

        internal bool IsValidInitialBalance(Account account)
        {
            return !(account.InitialBalance != null && account.InitialBalance != 0 && account.IsParent && account.ParentAccountCode != null);
        }
    }
}
