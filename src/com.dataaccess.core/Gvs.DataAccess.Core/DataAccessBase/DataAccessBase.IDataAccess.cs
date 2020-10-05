using Gvs.DataAccess.Core.ResourceFiles;
using System;
using System.Collections.Generic;
using System.Data;

namespace Gvs.DataAccess.Core
{
    /// <summary>
    /// Implementation of IDataAccess
    /// </summary>
    public abstract partial class DataAccessBase : IDataAccess
    {

        #region Transaction related Methods

        /// <summary>
        /// Gets the internal connection object of type IDBConnection.
        /// </summary>
        IDbConnection IDataAccess.DBConnection => DBConnection;

        /// <summary>
        /// Starts a new transaction on the current connection object.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        void IDataAccess.BeginTransaction()
        {
            try
            {
                BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.BeginTransaction, ex);
            }
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        void IDataAccess.RollbackTransaction()
        {
            try
            {
                RollbackTransaction();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.RollBackTransaction, ex);
            }
        }

        /// <summary>
        /// Commits the current transaction
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        void IDataAccess.CommitTransaction()
        {
            try
            {
                CommitTransaction();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.CommitTransaction, ex);
            }
        }

        /// <summary>
        /// Indicates whether or not there is an active transaction.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        bool IDataAccess.InTransaction => InTransaction;

        /// <summary>
        /// Set the timeout for command object. This property will help to execute long running queries
        /// </summary>
        int IDataAccess.CommandTimeOut
        {
            get => CommandTimeOut;
            set => CommandTimeOut = value;
        }

        #endregion Transaction related Methods

        #region DataSet returning methods

        /// <summary>
        /// Executes the query and returns the result as a data table inside a dataset
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="sql">The query to be executed executed.</param>
        /// <returns>A dataset with a single data table.</returns>
        DataSet IDataAccess.GetDataSet(string sql)
        {
            try
            {
                return GetDataSet(string.Empty, null, String.Empty, sql);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetDataSet, ex);
            }
        }

        /// <summary>
        /// Executes the query and returns the result as a data table inside a dataset
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="tableName">Name of the table in the returned dataset.</param>
        /// <param name="sql">The query to be executed executed.</param>
        /// <returns>A dataset with a single data table.</returns>
        DataSet IDataAccess.GetDataSet(string tableName, string sql)
        {
            try
            {
                return GetDataSet(tableName, null, String.Empty, sql);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetDataSet, ex);
            }
        }

        /// <summary>
        /// Gets the desired fields from a database table and returns the result as a data table inside a dataset.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="tableName">Name of the target table and name of the table in the returned dataset.</param>
        /// <param name="fields">Desired fields from the target table.</param>
        /// <returns>A dataset with a single data table.</returns>
        DataSet IDataAccess.GetDataSet(string tableName, IList<string> fields)
        {
            try
            {
                return GetDataSet(tableName, fields, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetDataSet, ex);
            }
        }

        /// <summary>
        /// Gets the desired fields from a database table and returns the result as a data table inside a dataset.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="tableName">Name of the target table and name of the table in the returned dataset.</param>
        /// <param name="fields">Desired fields from the target table.</param>
        /// <param name="filter">The exclusonary where condition for what rows to leave out.</param>
        /// <returns>A dataset with a single data table.</returns>
        DataSet IDataAccess.GetDataSet(string tableName, IList<string> fields, string filter)
        {
            try
            {
                return GetDataSet(tableName, fields, filter, string.Empty);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetDataSet, ex);
            }
        }

        #endregion DataSet returning methods

        #region DataReader returning methods

        /// <summary>
        /// Executes the query and returns a data reader.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="sql">The query statement to execute.</param>
        /// <returns>A data reader.</returns>
        IDataReader IDataAccess.GetReader(string sql)
        {
            try
            {
                return GetReader(string.Empty, null, string.Empty, sql);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetReader, ex);
            }
        }

        /// <summary>
        /// Gets a data reader for a given table and its desired fields.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="tableName">The desired table in the database.</param>
        /// <param name="fields">The desired fields of the table in the database.</param>
        /// <returns>A data reader.</returns>
        IDataReader IDataAccess.GetReader(string tableName, IList<string> fields)
        {
            try
            {
                return GetReader(tableName, fields, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetReader, ex);
            }
        }

        /// <summary>
        /// Gets a data reader for a given table and its desired fields.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="tableName">The desired table in the database.</param>
        /// <param name="fields">The desired fields of the table in the database.</param>
        /// <param name="filter">Indicates an exclusion condition for what rows to leave out.</param>
        /// <returns>A data reader.</returns>
        IDataReader IDataAccess.GetReader(string tableName, IList<string> fields, string filter)
        {
            try
            {
                return GetReader(tableName, fields, filter, string.Empty);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetReader, ex);
            }
        }

        IDataReader IDataAccess.GetReader(string sql, IList<QueryParameter> parameters)
        {
            try
            {
                return GetReader(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.GetReader, ex);
            }
        }

        #endregion DataReader returning methods

        #region Stored procedure execution methods

        /// <summary>
        /// Executes a stored procedure and returns the results as a data table inside a dataset.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="storedProcName">The stored procedure to execute.</param>
        /// <param name="parameters">A collection of stored procedures parameters.</param>
        /// <param name="tableName">The name to given to the returned data table.</param>
        /// <returns>A data table inside a dataset.</returns>
        DataSet IDataAccess.ExecuteProcDataSet(string storedProcName, IList<QueryParameter> parameters, string tableName)
        {
            try
            {
                return ExecuteProcDataSet(storedProcName, parameters, tableName);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.ExecuteProc, ex);
            }
        }

        ///// <summary>
        ///// Executes a stored procedure.
        ///// Throws Gvs.DataAccessException if an exception occurrs.
        ///// </summary>
        ///// <param name="storedProcName">The stored procedure to execute.</param>
        ///// <param name="parameters">A collection of stored procedures parameters.</param>
        void IDataAccess.ExecuteProcNonQuery(string storedProcName, IList<QueryParameter> parameters)
        {
            try
            {
                ExecuteProcNonQuery(storedProcName, parameters);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.ExecuteProc, ex);
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the results as a data reader.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="storedProcName">The stored procedure to execute.</param>
        /// <param name="parameters">A collection of stored procedures parameters.</param>
        /// <returns>A data reader.</returns>
        IDataReader IDataAccess.ExecuteProcDataReader(string storedProcName, IList<QueryParameter> parameters)
        {
            try
            {
                return ExecuteProcDataReader(storedProcName, parameters);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.ExecuteProc, ex);
            }
        }

        IList<T> IDataAccess.ExecuteProcDataObjects<T>(string storedProcName, IList<QueryParameter> parameters)
        {
            try
            {
                return ExecuteProcDataObjects<T>(storedProcName, parameters);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.ExecuteProc, ex);
            }
        }

        IList<object> IDataAccess.ExecuteProcDataObjects<T1, T2>(string storedProcName, IList<QueryParameter> parameters)
        {
            try
            {
                return ExecuteProcDataObjects<T1, T2>(storedProcName, parameters);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.ExecuteProc, ex);
            }
        }
        #endregion Stored procedure execution methods

        #region SQL execution methods

        /// <summary>
        /// Executes the query.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="sql">The query to execute.</param>
        void IDataAccess.ExecuteSQL(string sql)
        {
            try
            {
                ExecuteSQL(sql, new List<QueryParameter>());
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.ExecuteSQL, ex);
            }
        }

        /// <summary>
        /// Executes the query.
        /// Throws Gvs.DataAccessException if an exception occurrs.
        /// </summary>
        /// <param name="sql">The query to execute.</param>
        /// <param name="parameters">List of parameters</param>
        void IDataAccess.ExecuteSQL(string sql, IList<QueryParameter> parameters)
        {
            try
            {
                ExecuteSQL(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ExceptionMessages.ExecuteSQL, ex);
            }
        }

        ///// <summary>
        ///// Executes the query, and returns the first column of the first row in the
        ///// resultset returned by the query. Extra columns or rows are ignored.
        ///// Throws Gvs.DataAccessException if an exception occurrs.
        ///// </summary>
        ///// <param name="sql">The query to execute.</param>
        ///// <returns>The first row and first column of the resultset.</returns>
        //object IDataAccess.ExecuteScalar(string sql)
        //{
        //    try
        //    {
        //        return ExecuteScalar(sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataAccessException(ExceptionMessages.ExecuteSQL, ex);
        //    }

        //}

        #endregion SQL execution methods

        #region POCO returning methods

        T IDataAccess.Read<T>(object id)
        {
            return Read<T>(id);
        }

        IList<T> IDataAccess.ReadAll<T>()
        {
            return ReadAll<T>();
        }

        IList<T> IDataAccess.ReadAll<T>(string sql)
        {
            return ReadAll<T>(sql);
        }

        IList<T> IDataAccess.ReadAll<T>(IList<QueryParameter> parameters)
        {
            return ReadAll<T>(parameters);
        }

        IList<T> IDataAccess.ReadAll<T>(string sql, IList<QueryParameter> parameters)
        {
            return ReadAll<T>(sql, parameters);
        }

        //IDataReader IDataAccess.GetReader(string sql, IList<QueryParameter> parameters)
        //{
        //    throw new NotImplementedException();
        //}

        //IList<T> IDataAccess.ReadAll<T>()
        //{
        //    throw new NotImplementedException();
        //}

        //IList<T> IDataAccess.ReadAll<T>(string sql)
        //{
        //    throw new NotImplementedException();
        //}

        //IList<T> IDataAccess.ReadAll<T>(string tableName, IList<string> fieldNames)
        //{
        //    throw new NotImplementedException();
        //}

        //IList<T> IDataAccess.ReadAll<T>(string tableName, IList<string> fieldNames, string filter)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion POCO returning methods

        #region DML methods

        T IDataAccess.Insert<T>(T record)
        {
            return Insert(record);
        }

        T IDataAccess.Update<T>(T record)
        {
            return Update(record);
        }

        bool IDataAccess.Delete<T>(T record)
        {
            return Delete(record);
        }

        #endregion DML methods

    }
}
