using CPL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        bool Commit();
        void Rollback();
    }
}
