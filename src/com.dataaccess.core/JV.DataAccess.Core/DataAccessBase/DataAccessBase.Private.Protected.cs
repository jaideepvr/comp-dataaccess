using System.Collections.Generic;
using System.Text;
using System.Data;
using JV.DataAccess.Core.Helpers;
using JV.DataAccess.Core.QueryBuilder;
using JV.DataAccess.Core.Interfaces;
using System;

namespace JV.DataAccess.Core {
    /// <summary>
    /// Internal methods to implement IDataAccess methods
    /// </summary>
    public abstract partial class DataAccessBase {

        #region Transaction

        /// <summary>
        /// Begins a new transaction, opens the connection to database if required
        /// </summary>
        protected void BeginTransaction() {
            if (DBConnection.State == ConnectionState.Closed) {
                DBConnection.Open();
            }

            Transaction = DBConnection.BeginTransaction();
        }

        /// <summary>
        /// Commits the ongoing transaction, and closes the connection
        /// </summary>
        protected void CommitTransaction() {
            if (null != Transaction) {
                Transaction.Commit();

                DBConnection.Close();
            }
        }

        /// <summary>
        /// Rollbacks the connection and closes the connection
        /// </summary>
        protected void RollbackTransaction() {
            if (null != Transaction) {
                Transaction.Rollback();

                DBConnection.Close();
            }
        }

        /// <summary>
        /// Returns if the connection is within a transaction
        /// </summary>
        protected bool InTransaction => (Transaction != null);

        /// <summary>
        /// Gets/Sets the Command Timeout in seconds. When set to 0 this defaults to 30 seconds
        /// </summary>
        protected int CommandTimeOut {
            get => (commandTimeOut == 0) ? 30 : commandTimeOut;
            set => commandTimeOut = value;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Construct SELECT statement using th elist of parameters and filters
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="fields">list of fields</param>
        /// <param name="filter">list fo filters</param>
        /// <returns>SQL query string</returns>
        private string GetSQL(string tableName, IList<string> fields, string filter) {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            for (int index = 0; index < fields.Count; index++) {
                sql.Append(("t." + (fields[index] + ", ")));
            }

            sql = sql.Remove((sql.Length - 2), 2);

            sql.Append((" FROM " + (tableName + " t")));
            if (!string.IsNullOrEmpty(filter)) {
                sql.Append((" WHERE t." + filter));
            }

            return sql.ToString();
        }

        #endregion

        /// <summary>
        /// Prepare adapter to execute
        /// </summary>
        /// <param name="tableName">table name to use in dataset</param>
        /// <param name="fields">list of fields</param>
        /// <param name="filter">list of filters</param>
        /// <param name="sql">plain SQL</param>
        /// <returns>DataAdapter</returns>
        protected virtual IDbDataAdapter GetAdapter(string tableName, IList<string> fields, string filter, string sql) {
            string sqlQuery = string.Empty;

            if (string.IsNullOrEmpty(sql)) {
                sqlQuery = GetSQL(tableName, fields, filter);
            } else {
                sqlQuery = sql;
            }

            IDbDataAdapter adapter = CreateDataAdapter();
            adapter.SelectCommand = GetCommand(CommandType.Text, sqlQuery);

            return adapter;
        }

        protected virtual IDbDataAdapter CreateStoredProcDataAdapter(string storedProcName, IList<QueryParameter> parameters) {
            IDbCommand command = GetCommand(CommandType.StoredProcedure, storedProcName);
            LoadCommandParameters(command, parameters);
            return CreateDataAdapter(command);
        }

        protected virtual IDbCommand CreateStoredProcDataCommand(string storedProcName, IList<QueryParameter> parameters)
        {
            IDbCommand command = GetCommand(CommandType.StoredProcedure, storedProcName);
            LoadCommandParameters(command, parameters);
            return command;
        }

        protected virtual IQueryBuilder<T> GetQueryBuilder<T>() where T : class, new() {
            IQueryBuilder<T> queryBuilder = DataAccess.createQueryBuilder<T>(this);
            if (null == queryBuilder) {
                queryBuilder = new QueryBuilder<T>();
            }

            if (null != queryBuilder) {
                queryBuilder.Map = new Mapper<T>().Map(this);
            }

            return queryBuilder;
        }

        /// <summary>
        /// Get Command object
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        protected virtual IDbCommand GetCommand(CommandType commandType, string commandText) {
            IDbCommand command = DBConnection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.Transaction = Transaction;
            if (CommandTimeOut != 0) {
                command.CommandTimeout = CommandTimeOut;
            }

            return command;
        }

        #region Data Access

        /// <summary>
        /// Executes the query and returns the result as a data table inside a dataset
        /// </summary>
        /// <param name="tableName">data table name</param>
        /// <param name="fields">List of fields</param>
        /// <param name="filter">List of filters</param>
        /// <param name="sql">plain SQL script</param>
        /// <returns>A dataset with a single data table</returns>
        protected DataSet GetDataSet(string tableName, IList<string> fields, string filter, string sql) {
            IDbDataAdapter adapter = GetAdapter(tableName, fields, filter, sql);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            if (!string.IsNullOrEmpty(tableName)) {
                dataSet.Tables[0].TableName = tableName;
            }
            return dataSet;
        }

        /// <summary>
        /// Prepare reader
        /// </summary>
        /// <param name="tableName">table name to use in dataset</param>
        /// <param name="fields">list of fields</param>
        /// <param name="filter">list of filters</param>
        /// <param name="sql">plain SQL</param>
        /// <returns>data reader</returns>
        protected IDataReader GetReader(string tableName, IList<string> fields, string filter, string sql) {
            string sqlQuery = string.Empty;

            HandleOpeningConnection();
            if (string.IsNullOrEmpty(sql)) {
                sqlQuery = GetSQL(tableName, fields, filter);
            } else {
                sqlQuery = sql;
            }

            IDbCommand command = GetCommand(CommandType.Text, sqlQuery);
            return new DataReader(command.ExecuteReader(CommandBehavior.Default));
        }

        ///// <summary>
        ///// Executes the query, and returns the first column of the first row in the
        ///// resultset returned by the query. Extra columns or rows are ignored.
        ///// </summary>
        ///// <param name="sql">plain SQL script</param>
        ///// <returns>first value</returns>
        //private object ExecuteScalar(string sql)
        //{
        //    try
        //    {
        //        HandleOpeningConnection();
        //        return GetCommand(CommandType.Text, sql).ExecuteScalar();
        //    }
        //    finally
        //    {
        //        HandleClosingConnection();
        //    }
        //}

        /// <summary>
        /// Execute plain SQL statement with parameter
        /// </summary>
        /// <param name="sql">SQL statement.</param>
        /// <param name="parameters">List of parameters</param>
        protected void ExecuteSQL(string sql, IList<QueryParameter> parameters) {
            IDbCommand command = GetCommand(CommandType.Text, sql);

            LoadCommandParameters(command, parameters);

            try {
                HandleOpeningConnection();
                command.ExecuteNonQuery();
                ExtractReturnedParameterValue(parameters, command);
            } finally {
                HandleClosingConnection();
            }
        }

        /// <summary>
        /// Execute the sql and return a DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected IDataReader GetReader(string sql, IList<QueryParameter> parameters) {
            HandleOpeningConnection();

            IDbCommand command = GetCommand(CommandType.Text, sql);
            LoadCommandParameters(command, parameters);
            return new DataReader(command.ExecuteReader(CommandBehavior.Default));
        }

        /// <summary>
        /// Execute stored procs which are not returning any data
        /// </summary>
        /// <param name="storedProcName">name of the proc</param>
        /// <param name="parameters">List of parameters</param>
        protected void ExecuteProcNonQuery(string storedProcName, IList<QueryParameter> parameters) {
            IDbCommand command = CreateStoredProcDataCommand(storedProcName, parameters);

            try {
                HandleOpeningConnection();
                command.ExecuteNonQuery();
                ExtractReturnedParameterValue(parameters, command);
            } finally {
                HandleClosingConnection();
            }
        }

        /// <summary>
        /// Extract prameter values for object
        /// </summary>
        /// <param name="parameters">parameter collection</param>
        /// <param name="command">command object</param>
        protected virtual void ExtractReturnedParameterValue(IList<QueryParameter> parameters, IDbCommand command) {
            foreach (QueryParameter parameter in parameters) {
                switch (parameter.Direction) {
                    case ParameterDirection.InputOutput:
                    case ParameterDirection.Output:
                    case ParameterDirection.ReturnValue:
                        parameter.Value = ExtractParameterValue(command.Parameters[parameter.ParameterName]);
                        break;
                }
            }
        }

        /// <summary>
        /// Execute the stored proc and return result as dataset
        /// </summary>
        /// <param name="storedProcName">Name of the proc</param>
        /// <param name="parameters">list of parameters</param>
        /// <param name="tableName">dataset table name</param>
        /// <returns></returns>
        protected DataSet ExecuteProcDataSet(string storedProcName, IList<QueryParameter> parameters, string tableName) {
            DataSet dataSet = new DataSet();

            IDbDataAdapter adapter = CreateStoredProcDataAdapter(storedProcName, parameters);

            FillDataSet(adapter, dataSet, tableName);
            ExtractReturnedParameterValue(parameters, adapter.SelectCommand);

            return dataSet;
        }

        /// <summary>
        /// Execute the stored proc and return result as data reader
        /// </summary>
        /// <param name="storedProcName">Name of the proc</param>
        /// <param name="parameters">list of parameters</param>
        /// <returns>data reader</returns>
        protected IDataReader ExecuteProcDataReader(string storedProcName, IList<QueryParameter> parameters) {
            IDbCommand command = CreateStoredProcDataCommand(storedProcName, parameters);
            IDataReader reader = null;

            HandleOpeningConnection();
            reader = command.ExecuteReader(CommandBehavior.Default);
            ExtractReturnedParameterValue(parameters, command);

            return reader;
        }

        /// <summary>
        /// Execute the stored proc and return result as data reader
        /// </summary>
        /// <param name="storedProcName">Name of the proc</param>
        /// <param name="parameters">list of parameters</param>
        /// <returns>data reader</returns>
        protected IList<T> ExecuteProcDataObjects<T>(string storedProcName, IList<QueryParameter> parameters) where T : class, new() {
            IDataReader reader = ExecuteProcDataReader(storedProcName, parameters);
            Transform<T> entityReader = new Transform<T>(this, reader);
            List<T> entities = entityReader.ReadAll(reader);

            reader.Close();

            return entities;
        }

        /// <summary>
        /// Execute the stored proc and return result as data reader
        /// </summary>
        /// <param name="storedProcName">Name of the proc</param>
        /// <param name="parameters">list of parameters</param>
        /// <returns>data reader</returns>
        protected IList<object> ExecuteProcDataObjects<T1, T2>(string storedProcName, IList<QueryParameter> parameters)
            where T1 : class, new()
            where T2 : class, new() {
            var allLists = new List<object>();

            IDataReader reader = ExecuteProcDataReader(storedProcName, parameters);
            Transform<T1> entityReader1 = new Transform<T1>(this, reader);
            List<T1> entities1 = entityReader1.ReadAll(reader);
            allLists.Add(entities1);

            if (!reader.NextResult()) {
                throw new DataAccessException("Index out of range: No second DataReader available");
            }
            Transform<T2> entityReader2 = new Transform<T2>(this, reader);
            List<T2> entities2 = entityReader2.ReadAll(reader);
            allLists.Add(entities2);

            reader.Close();

            return allLists;
        }

        /// <summary>
        /// Check and close DB connection
        /// </summary>
        private void HandleClosingConnection() {
            if (ClosingConnectionAllowed()) {
                DBConnection.Close();
            }
        }

        /// <summary>
        /// Check and open DB connection
        /// </summary>
        private void HandleOpeningConnection() {
            if (OpeningConnectionAllowed()) {
                DBConnection.Open();
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// Property to check connection closing option
        /// </summary>
        /// <returns></returns>
        protected bool ClosingConnectionAllowed() => (!InTransaction);

        /// <summary>
        /// Property to check open connection option
        /// </summary>
        /// <returns></returns>
        protected bool OpeningConnectionAllowed() => (DBConnection.State != ConnectionState.Open);

        /// <summary>
        /// Load command parameter from list
        /// </summary>
        /// <param name="cmd">command object</param>
        /// <param name="parameters">List of parameters</param>
        protected virtual void LoadCommandParameters(IDbCommand cmd, IList<QueryParameter> parameters) {
            if (parameters != null && parameters.Count > 0) {
                foreach (QueryParameter parameter in parameters) {
                    Parameter param = this.createParameter(parameter);
                    cmd.Parameters.Add(param.ConvertToSpecificDBType());
                }
            }
        }

        #endregion

        protected virtual T Read<T>(object id) where T : class, new() {
            var queryBuilder = GetQueryBuilder<T>();
            if (null == queryBuilder.Map.PrimaryKey) {
                throw new DataAccessException($"No Primary Key defined for entity {queryBuilder.Map.EntityName}");
            }
            var parameters = new List<QueryParameter>
            {
                new QueryParameter(queryBuilder.Map.PrimaryKey.FieldName, ParameterDirection.Input, id)
                {
                    ParameterType = queryBuilder.Map.PrimaryKey.PropertyType
                }
            };

            string sql = queryBuilder.SelectStatement(parameters);
            IDataReader reader = GetReader(sql, parameters);
            Transform<T> entityReader = new Transform<T>(this, reader);
            T entity = entityReader.Read(reader);

            reader.Close();

            return entity;
        }

        protected virtual IList<T> ReadAll<T>() where T : class, new() {
            string sql = GetQueryBuilder<T>().SelectAllStatement;
            IDataReader reader = GetReader(string.Empty, null, string.Empty, sql);
            Transform<T> entityReader = new Transform<T>(this, reader);
            List<T> entities = entityReader.ReadAll(reader);

            reader.Close();

            return entities;
        }

        protected virtual IList<T> ReadAll<T>(string sql) where T : class, new() {
            IDataReader reader = GetReader(string.Empty, null, string.Empty, sql);
            Transform<T> entityReader = new Transform<T>(this, reader);
            List<T> entities = entityReader.ReadAll(reader);

            reader.Close();

            return entities;
        }

        protected virtual IList<T> ReadAll<T>(string sql, IList<QueryParameter> parameters) where T : class, new() {
            IDataReader reader = GetReader(sql, parameters);
            Transform<T> entityReader = new Transform<T>(this, reader);

            List<T> entities = entityReader.ReadAll(reader);
            reader.Close();

            return entities;
        }

        protected virtual IList<T> ReadAll<T>(IList<QueryParameter> parameters) where T : class, new() {
            string sql = GetQueryBuilder<T>().SelectStatement(parameters);
            IDataReader reader = GetReader(sql, parameters);
            Transform<T> entityReader = new Transform<T>(this, reader);

            List<T> entities = entityReader.ReadAll(reader);
            reader.Close();

            return entities;
        }

        protected virtual T Insert<T>(T record) where T : class, new() {
            var queryBuilder = GetQueryBuilder<T>();
            var insertSql = queryBuilder.InsertStatement;
            var parameters = new List<QueryParameter>();
            var recordType = new T().GetType();

            queryBuilder.Map.ElementMap.ForEach(map => {
                if (map.IsNative && !map.IsPrimaryKey) {
                    parameters.Add(new QueryParameter(map.FieldName, ParameterDirection.Input, GetObjectValue(recordType, record, map.PropertyName)) {
                        ParameterType = map.PropertyType
                    });
                }
            });

            ExecuteSQL(insertSql, parameters);

            return record;
        }

        protected virtual T Update<T>(T record) where T : class, new() {
            var queryBuilder = GetQueryBuilder<T>();
            var updateSql = queryBuilder.UpdateStatement;
            var parameters = new List<QueryParameter>();
            var recordType = new T().GetType();

            queryBuilder.Map.ElementMap.ForEach(map => {
                if (map.IsNative) {
                    parameters.Add(new QueryParameter(map.FieldName, ParameterDirection.Input, GetObjectValue(recordType, record, map.PropertyName)) {
                        ParameterType = map.PropertyType
                    });
                }
            });

            ExecuteSQL(updateSql, parameters);

            return record;
        }

        protected virtual bool Delete<T>(T record) where T : class, new() {
            try {
                var queryBuilder = GetQueryBuilder<T>();
                var updateSql = queryBuilder.DeleteStatement;
                var parameters = new List<QueryParameter>();
                var recordType = new T().GetType();

                queryBuilder.Map.ElementMap.ForEach(map => {
                    if (map.IsNative) {
                        parameters.Add(new QueryParameter(map.FieldName, ParameterDirection.Input, GetObjectValue(recordType, record, map.PropertyName)) {
                            ParameterType = map.PropertyType
                        });
                    }
                });

                ExecuteSQL(updateSql, parameters);

                return true;

            } catch (Exception) {
                return false;
            }
        }

        private object GetObjectValue<T>(Type recordType, T record, string propertyName) where T : class, new() {
            return recordType.GetProperty(propertyName).GetValue(record);
        }

    }

}
