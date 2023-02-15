using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.DataAccess.XTests.DataFixtures
{
    public sealed class MysqlDataFixture: IDisposable
    {

        private string _connectionString = @"Server=localhost;Database=abc;Uid=****;Pwd=*****";

        public MysqlDataFixture()
        {
            string setupSql = File.ReadAllText(@"DataFixtures\Setup.sql");
            executeSql(_connectionString, setupSql);
        }

        public void Dispose()
        {
            string cleanupSql = File.ReadAllText(@"DataFixtures\Cleanup.sql");
            executeSql(_connectionString, cleanupSql);

            GC.SuppressFinalize(this);
        }

        private void executeSql(string connectionString, string sql)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(sql);
            command.Connection = connection;
            command.ExecuteNonQuery();
        }

    }
}
