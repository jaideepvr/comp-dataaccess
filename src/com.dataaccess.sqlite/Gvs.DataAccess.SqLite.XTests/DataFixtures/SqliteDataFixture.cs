using System.Data.SQLite;
using System;
using System.IO;

namespace Gvs.DataAccess.XTests.DataFixtures
{
    public sealed class SqliteDataFixture: IDisposable
    {

        private string _connectionString = @"URI=file:E:\Projects\GVS\Common\SQLiteDB\test.db";

        public SqliteDataFixture()
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
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(sql);
            command.Connection = connection;
            command.ExecuteNonQuery();
        }

    }
}
