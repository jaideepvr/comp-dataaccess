using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace Gvs.DataAccess.Core
{
    /// <summary>
    /// DataAccess Interface methods
    /// 
    /// 
    /// </summary>
    public interface IDataAccess
    {
        IDbConnection DBConnection { get; }

        //Transaction related methods
        void BeginTransaction();
        void RollbackTransaction();
        void CommitTransaction();
        bool InTransaction { get; }
        int CommandTimeOut { get; set; }

        //Dataset related methods
        DataSet GetDataSet(string sql);
        DataSet GetDataSet(string tableName, string sql);
        DataSet GetDataSet(string tableName, IList<string> fields);
        DataSet GetDataSet(string tableName, IList<string> fields, string filter);

        ////GetReader
        IDataReader GetReader(string sql);
        IDataReader GetReader(string tableName, IList<string> fields);
        IDataReader GetReader(string tableName, IList<string> fields, string filter);
        IDataReader GetReader(string sql, IList<QueryParameter> parameters);

        //T Read<T>(string sql) where T : class, new();
        T Read<T>(object id) where T : class, new();
        IList<T> ReadAll<T>() where T : class, new();
        IList<T> ReadAll<T>(string sql) where T : class, new();
        IList<T> ReadAll<T>(IList<QueryParameter> parameters) where T : class, new();
        IList<T> ReadAll<T>(string sql, IList<QueryParameter> parameters) where T : class, new();

        ////ExecProc
        void ExecuteProcNonQuery(string storedProcName, IList<QueryParameter> parameters);
        DataSet ExecuteProcDataSet(string storedProcName, IList<QueryParameter> parameters, string tableName);
        IDataReader ExecuteProcDataReader(string storedProcName, IList<QueryParameter> parameters);
        IList<T> ExecuteProcDataObjects<T>(string storedProcName, IList<QueryParameter> parameters) where T : class, new();
        IList<object> ExecuteProcDataObjects<T1, T2>(string storedProcName, IList<QueryParameter> parameters) where T1 : class, new() where T2 : class, new();

        //Sql
        void ExecuteSQL(string sql);
        void ExecuteSQL(string sql, IList<QueryParameter> parameters);
        //object ExecuteScalar(string sql);

        //// Execute Insert Record Sql
        //object InsertRecord(string sql, List<Parameter> parameters);
        T Insert<T>(T record) where T : class, new();
        T Update<T>(T record) where T : class, new();
        bool Delete<T>(T record) where T : class, new();

    }
}
