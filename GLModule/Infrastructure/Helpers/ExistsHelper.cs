using GLModule.Data;
using GLModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace GLModule.Infrastructure.Helpers
{
    public class ExistsHelper
    {
        private readonly ApplicationDbContext _context;

        public ExistsHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        internal bool RowExists<T>(int id, string propertyName)
        {
            var dbSetProperty = _context.GetType().GetProperty(propertyName);

            if (dbSetProperty != null)
            {
                var dbSet = (IEnumerable<T>)dbSetProperty.GetValue(_context);

                var entityIdProperty = typeof(T).GetProperty($"{id}");
                if (entityIdProperty != null)
                {
                    var entityExists = dbSet?.Any(e => (int)entityIdProperty.GetValue(e, null) == id) ?? false;
                    return entityExists;
                }
            }

            return false;
        }
        internal bool AccountTransactionExists(int accountCode)
        {
            return (_context.AccountTransactions?.Any(e => e.AccountCode == accountCode)).GetValueOrDefault();
        }
        internal async Task<AccountTransaction> GetAccountTransaction(Guid? accountTransactionId)
        {
            return await _context.AccountTransactions.FirstOrDefaultAsync(m => m.AccountTransactionId == accountTransactionId);
        }
        internal bool RowExistsWithCondition<TEntity>(Expression<Func<TEntity, bool>> condition, string tableName) where TEntity : class
        {
            return _context.Set<TEntity>().Any(condition);
        }
    }
}
