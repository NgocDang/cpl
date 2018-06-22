using CPL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Infrastructure.Interfaces
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;
        void SyncObjectsStatePostCommit();
    }
}
