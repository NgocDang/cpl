using CPL.Common.Interfaces;
using CPL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CPL.Infrastructure.Repositories
{
    public sealed class QueryFluent<TEntity> : IQueryFluent<TEntity> where TEntity : class, IObjectState
    {
        private readonly Expression<Func<TEntity, bool>> _expression;
        private readonly List<Expression<Func<TEntity, object>>> _includes;
        private readonly Repository<TEntity> _repository;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderBy;

        public QueryFluent(Repository<TEntity> repository)
        {
            this._repository = repository;
            this._includes = new List<Expression<Func<TEntity, object>>>();
        }

        public QueryFluent(Repository<TEntity> repository, IQueryObject<TEntity> queryObject) : this(repository)
        {
            this._expression = queryObject.Query();
        }

        public QueryFluent(Repository<TEntity> repository, Expression<Func<TEntity, bool>> expression) : this(repository)
        {
            this._expression = expression;
        }

        public IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            this._orderBy = orderBy;
            return this;
        }

        public IIncludableQueryable<TEntity, TResult> Include<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return this._repository.Include<TResult>(expression);
        }

        public IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return this._repository.SelectQuery(query, parameters).AsQueryable();
        }
    }
}
