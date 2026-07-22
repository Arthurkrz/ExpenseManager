using ExpenseManager.Core.Common;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Entities;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using LinqKit;

namespace ExpenseManager.Infrastructure.Repositories
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository 
    {
        private readonly Context _context;
        
        public ExpenseRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Expense>> GetExpensesPagedAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = NormalizePageSize(pageSize);

            var query = _context.Set<Expense>().AsNoTracking()
                .OrderByDescending(e => e.ExpenseDate);

            var totalCount = await query.CountAsync();

            var items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PaginatedResult<Expense>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PaginatedResult<Expense>> GetExpensesWithFilterPagedAsync(Expression<Func<Expense, bool>> predicate, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context.Set<Expense>().AsNoTracking()
                .Where(predicate).OrderByDescending(e => e.ExpenseDate);

            var totalCount = await query.CountAsync();

            var items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PaginatedResult<Expense>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        private static int NormalizePageSize(int pageSize)
        {
            if (pageSize <= 0) return 10;

            return pageSize > 100 ? 100 : pageSize;
        }
    }
}
