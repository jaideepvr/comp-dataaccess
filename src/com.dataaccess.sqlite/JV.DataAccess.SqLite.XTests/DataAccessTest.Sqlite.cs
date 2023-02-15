using JV.DataAccess.Core.XTests;
using JV.DataAccess.XTests.DataFixtures;
using Xunit;

namespace JV.DataAccess.Sqlite.XTests
{
    public class SqliteDataAccessTest: DataAccessTestBase, IClassFixture<SqliteDataFixture>
    {

        protected override string ConnectionString => @"provider=JV.DataAccess.Sqlite; provider connection string='URI=file:E:\Projects\Gvs\Common\SQLiteDB\test.db';";

        protected override string AllUsersSql => "SELECT user_id, user_name, password, first_name, last_name, date_of_birth FROM user";

        protected override string AllUsersSqlTableName => "user";

        protected override string InsertNewUserSql => "INSERT INTO user (user_name, password, first_name, last_name, date_of_birth) VALUES ('sravan', 'a', 'Sravan Kumar', 'Vinakota', '1999-08-09')";

        protected override string SingleUserSelectSql => "SELECT user_id, user_name, password, first_name, last_name FROM user WHERE user_name = 'sravan'";

        protected override string DeleteUserSql => "DELETE FROM user WHERE user_name = @p_userName";

        protected override string ParameterizedSelectUserSql => "SELECT user_id, user_name, password, first_name, last_name FROM user WHERE user_name = @p_userName";

    }
}
