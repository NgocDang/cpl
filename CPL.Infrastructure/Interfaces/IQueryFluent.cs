using CPL.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CPL.Infrastructure.Interfaces
{
    public interface IQueryFluent<TEntity> where TEntity : IObjectState
    {
        IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IIncludableQueryable<TEntity, TResult> Include<TResult>(Expression<Func<TEntity, TResult>> expression);
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}
