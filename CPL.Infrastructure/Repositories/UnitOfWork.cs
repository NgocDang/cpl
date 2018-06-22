using CommonServiceLocator;
using CPL.Common.Interfaces;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPL.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        private IDataContextAsync _dataContext;
        private bool _disposed;
        private DbTransaction _transaction;
        private Dictionary<string, dynamic> _repositories;

        public UnitOfWork(IDataContextAsync dataContext)
        {
            this._dataContext = dataContext;
            this._repositories = new Dictionary<string, dynamic>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this._dataContext != null)
                {
                    this._dataContext.Dispose();
                    this._dataContext = null;
                }
            }

            // release any unmanaged objects
            // set the object references to null

            this._disposed = true;
        }

        public int SaveChanges()
        {
            return this._dataContext.SaveChanges();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }

            return RepositoryAsync<TEntity>();
        }

        public Task<int> SaveChangesAsync()
        {
            return this._dataContext.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return this._dataContext.SaveChangesAsync(cancellationToken);
        }

        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
            }

            if (this._repositories == null)
            {
                this._repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (this._repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)this._repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            this._repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), this._dataContext, this));

            return this._repositories[type];
        }

        public bool Commit()
        {
            this._transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            this._transaction.Rollback();
            this._dataContext.SyncObjectsStatePostCommit();
        }
    }
}
