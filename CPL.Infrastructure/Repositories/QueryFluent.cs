using CPL.Common.Interfaces;
using CPL.Infrastructure.Interfaces;
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

        public IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            this._includes.Add(expression);
            return this;
        }

        public IEnumerable<TEntity> SelectPage(int page, int pageSize, out int totalCount)
        {
            totalCount = this._repository.Select(this._expression).Count();
            return this._repository.Select(this._expression, this._orderBy, this._includes, page, pageSize);
        }

        public IEnumerable<TEntity> Select()
        {
            return this._repository.Select(this._expression, this._orderBy, this._includes);
        }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return this._repository.Select(this._expression, this._orderBy, this._includes).Select(selector);
        }

        public async Task<IEnumerable<TEntity>> SelectAsync()
        {
            return await this._repository.SelectAsync(this._expression, this._orderBy, this._includes);
        }

        public IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return this._repository.SelectQuery(query, parameters).AsQueryable();
        }
    }
}
