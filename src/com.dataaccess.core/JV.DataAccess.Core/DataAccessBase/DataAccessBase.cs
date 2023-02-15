using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace JV.DataAccess.Core
{
    /// <summary>
    /// Data access based class, use this base class to inherit Oracle and MSSQL data access class
    /// </summary>
    public abstract partial class DataAccessBase : IDisposable
    {
        // Local variable
        private bool isDisposed = false;
        protected IDbConnection DBConnection { get; private set; }
        protected IDbTransaction Transaction { get; set; }

        internal string ProviderAssembly { get; set; }

        private int commandTimeOut;

        // Base class methods
        protected abstract IDbDataAdapter CreateDataAdapter();
        protected abstract IDbDataAdapter CreateDataAdapter(IDbCommand command);
        protected abstract void FillDataSet(IDbDataAdapter adapter, DataSet dataSet, string tableName);

        protected abstract object ExtractParameterValue(object parameter);

        protected DataAccessBase(IDbConnection dbConnection)
        {
            DBConnection = dbConnection;
        }

        /// <summary>
        /// Distructor to dispose the current object
        /// </summary>
        ~DataAccessBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose connection
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed) 
            {
                if (disposing)
                {
                    if (DBConnection != null)
                    {
                        DBConnection.Close();
                    }

                }
            }
            this.isDisposed = true;
        }

        /// <summary>
        /// Force to dispose the connection from calling object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
