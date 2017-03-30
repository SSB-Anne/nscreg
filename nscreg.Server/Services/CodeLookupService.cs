﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nscreg.Data.Entities;
using nscreg.Data.Repositories;
using nscreg.Server.Models.Lookup;

namespace nscreg.Server.Services
{
    public class CodeLookupService<T> where T: CodeLookupBase
    {
        private DbContext _context;
        private readonly CodeLookupRepository<T> _repository;

        public CodeLookupService(DbContext context)
        {
            _context = context;
            _repository = new CodeLookupRepository<T>(_context);
        }

        public virtual async Task<List<CodeLookupVm>> List(bool showDeleted = false, Expression<Func<T, bool>> predicate = null)
        {
            var query = _repository.List(showDeleted);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await ToViewModel(query);
        }

        public virtual async Task<List<CodeLookupVm>> Search(string code, int limit = 5, bool showDeleted = false)
        {
            return await ToViewModel(_repository.List(showDeleted).Where(v => v.Code.StartsWith(code)).OrderBy(v => v.Code).Take(limit));
        }

        protected virtual async Task<List<CodeLookupVm>> ToViewModel(IQueryable<T> query)
        {
            return await query.Select(v => new CodeLookupVm()
            {
                Id = v.Id,
                Code = v.Code,
                Name = v.Name,
            }).ToListAsync();
        }
    }
}
