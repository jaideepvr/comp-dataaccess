using Gvs.DataAccess.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gvs.DataAccess.Mysql
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
