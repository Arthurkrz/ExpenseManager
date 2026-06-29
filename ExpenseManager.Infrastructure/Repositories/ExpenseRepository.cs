using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseManager.Infrastructure.Repositories
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {
        private readonly Context _context;

        public ExpenseRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetExpensesWithFilterAsync(Expression<Func<Expense, bool>> predicate) =>
            await _context.Set<Expense>().Where(predicate).ToListAsync();

        public async Task<Expense> GetByIdAsync(Guid id) =>
            await (await GetAllAsync()).FirstOrDefaultAsync(b => b.Id == id);
    }
}
