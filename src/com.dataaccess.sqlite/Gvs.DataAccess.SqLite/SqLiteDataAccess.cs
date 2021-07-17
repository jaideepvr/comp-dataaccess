using Gvs.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace Gvs.DataAccess.SqLite
{
    public class SqLiteDataAccess : DataAccessBase
    {

        public SqLiteDataAccess(string connectionString) : base(new SQLiteConnection(connectionString))
        {
        }

        protected override IDbDataAdapter CreateDataAdapter() => new SQLiteDataAdapter();

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command) => new SQLiteDataAdapter((SQLiteCommand)command);

        protected override IDbDataAdapter CreateStoredProcDataAdapter(string storedProcName, IList<QueryParameter> parameters) => throw new NotSupportedException("SQLite does not support stored procedures");

        protected override IDbCommand CreateStoredProcDataCommand(string storedProcName, IList<QueryParameter> parameters) => throw new NotSupportedException("SQLite does not support stored procedures");

        protected override object ExtractParameterValue(object parameter) => throw new NotSupportedException();

        protected override void FillDataSet(IDbDataAdapter adapter, DataSet dataSet, string tableName) => ((SQLiteDataAdapter)adapter).Fill(dataSet, tableName);

    }
}
