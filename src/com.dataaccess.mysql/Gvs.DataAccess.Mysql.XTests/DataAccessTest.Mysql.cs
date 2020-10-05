using Gvs.DataAccess.Core.XTests;
using Gvs.DataAccess.XTests.DataFixtures;
using Xunit;

namespace Gvs.DataAccess.Mysql.XTests
{
    public class MysqlDataAccessTest: DataAccessTestBase, IClassFixture<MysqlDataFixture>
    {

        protected override string ConnectionString => "provider=Gvs.DataAccess.Mysql; provider connection string='Server=localhost;Database=smdev;Uid=root;Pwd=WinterGo@123';";

        protected override string AllUsersSql => "SELECT user_id, user_name, password, first_name, last_name, date_of_birth FROM user";

        protected override string AllUsersSqlTableName => "user";

        protected override string InsertNewUserSql => "INSERT INTO user (user_name, password, first_name, last_name, date_of_birth) VALUES ('sravan', 'a', 'Sravan Kumar', 'Vinakota', '1999-08-09')";

        protected override string SingleUserSelectSql => "SELECT user_id, user_name, password, first_name, last_name FROM user WHERE user_name = 'sravan'";

        protected override string DeleteUserSql => "DELETE FROM user WHERE user_name = @p_userName";

        protected override string ParameterizedSelectUserSql => "SELECT user_id, user_name, password, first_name, last_name FROM user WHERE user_name = @p_userName";

    }
}
