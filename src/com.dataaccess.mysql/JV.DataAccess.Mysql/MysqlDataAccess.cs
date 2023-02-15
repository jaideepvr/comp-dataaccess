using JV.DataAccess.Core;
using MySql.Data.MySqlClient;
using System.Data;

namespace JV.DataAccess.Mysql
{
    public class MysqlDataAccess : DataAccessBase
    {

        public MysqlDataAccess(string connectionString)
            : base(new MySqlConnection(connectionString))
        {
        }

        protected override IDbDataAdapter CreateDataAdapter() => new MySqlDataAdapter();

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command) => new MySqlDataAdapter((MySqlCommand)command);

        protected override object ExtractParameterValue(object parameter) => ((MySqlParameter)parameter).Value;

        protected override void FillDataSet(IDbDataAdapter adapter, DataSet dataSet, string tableName) => ((MySqlDataAdapter)adapter).Fill(dataSet, tableName);

    }
}
