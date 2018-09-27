using CPL.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CPL.Infrastructure.Interfaces
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;
        void SyncObjectsStatePostCommit();
        void ExecuteSqlCommand(RawSqlString sql, params object[] parameters);
        DataSet ExecuteStoredProcedure(string sqlCommandText, IList<SqlParameter> parameters);
    }
}
