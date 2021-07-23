using Jsits.DataAccess.Core.Helpers;
using Jsits.DataAccess.Core.XTests.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Jsits.DataAccess.Core.XTests
{
    public abstract class DataAccessTestBase
    {

        protected abstract string ConnectionString { get; }

        protected virtual IDataAccess Connection
        {
            get
            {
                var connection = DataAccess.createConnection(ConnectionString);

                Assert.NotNull(connection);
                Assert.IsAssignableFrom<IDataAccess>(connection);

                return connection;
            }
        }

        #region Dataset tests

        [Fact]
        public void TestReadDataSet()
        {
            var userSet = Connection.GetDataSet(AllUsersSql);

            Assert.NotNull(userSet);
            Assert.True(userSet.Tables[0].Rows.Count > 0);
        }

        [Fact]
        public void TestReadDataSetWithTableName()
        {
            var userSet = Connection.GetDataSet("Users", AllUsersSql);

            Assert.NotNull(userSet);
            Assert.Equal("Users", userSet.Tables[0].TableName);
            Assert.True(userSet.Tables[0].Rows.Count > 0);
        }

        [Fact]
        public void TestReadDataSetWithFields()
        {
            var userSet = Connection.GetDataSet(AllUsersSqlTableName, new List<string> { "user_id", "user_name", "password", "first_name", "last_name" });

            Assert.NotNull(userSet);
            Assert.Equal(AllUsersSqlTableName, userSet.Tables[0].TableName);
            Assert.True(userSet.Tables[0].Rows.Count > 0);
            Assert.Equal(5, userSet.Tables[0].Columns.Count);
        }

        [Theory]
        [InlineData("jaideep")]
        public void TestReadDataSetWithFieldsAndFilter(string userName)
        {
            var userSet = Connection.GetDataSet(AllUsersSqlTableName, new List<string> { "user_id", "user_name", "password", "first_name", "last_name" }, $"user_name ='{userName}'");

            Assert.NotNull(userSet);
            Assert.Equal(AllUsersSqlTableName, userSet.Tables[0].TableName);
            Assert.True(userSet.Tables[0].Rows.Count > 0);
            Assert.Equal(5, userSet.Tables[0].Columns.Count);
        }

        #endregion

        #region DataReader Tests

        [Fact]
        public void TestReadReader()
        {
            var userReader = Connection.GetReader(AllUsersSql);

            Assert.NotNull(userReader);
            Assert.Equal(6, userReader.FieldCount);
        }

        [Fact]
        public void TestReadReaderWithFields()
        {
            var userReader = Connection.GetReader(AllUsersSqlTableName, new List<string> { "user_id", "user_name", "password", "first_name", "last_name" });

            Assert.NotNull(userReader);
            Assert.Equal(5, userReader.FieldCount);
        }

        [Theory]
        [InlineData("jaideep")]
        public void TestReadReaderWithFieldsAndFilter(string userName)
        {
            var userReader = Connection.GetReader(AllUsersSqlTableName, new List<string> { "user_id", "user_name", "password", "first_name", "last_name" }, $"user_name = '{userName}'");

            Assert.NotNull(userReader);
            Assert.Equal(5, userReader.FieldCount);
        }

        #endregion DataReader Tests

        #region POCO Tests

        [Fact]
        public void TestPocoReader()
        {
            var users = Connection.ReadAll<User>();

            Assert.IsType<List<User>>(users);
            Assert.True(users.Count > 0);

            Assert.Equal(1, users[0].UserId);
            Assert.Equal("jaideep", users[0].UserName);

            Assert.Equal(2, users[1].UserId);
            Assert.Equal("sridevi", users[1].UserName);
        }

        [Theory]
        [InlineData(1, "jaideep")]
        [InlineData(2, "sridevi")]
        public void TestSingleRecordPocoReader(int id, string userName)
        {
            var user = Connection.Read<User>(id);
            Assert.NotNull(user);
            Assert.Equal(id, user.UserId);
            Assert.Equal(userName, user.UserName);
        }

        [Fact]
        public void TestPocoReaderWithQuery()
        {
            var users = Connection.ReadAll<User>(AllUsersSql);

            Assert.NotNull(users);
            Assert.IsType<List<User>>(users);
            Assert.True(users.Count > 0);
        }

        [Theory]
        [InlineData("jaideep")]
        public void TestPocoReaderWithParameters(string userName)
        {
            var queryParams = new List<QueryParameter>()
            {
                new QueryParameter("user_name", ParameterDirection.Input, userName) { ParameterType = typeof(String) }
            };

            var users = Connection.ReadAll<User>(queryParams);

            Assert.NotNull(users);
            Assert.IsType<List<User>>(users);
            Assert.Equal(1, users.Count);
        }

        [Theory]
        [InlineData("ratnam", "a", "Ratnam", "Vinakota", "1943-05-27")]
        public void TestPocoInsertRecord(string userName, string password, string firstName, string lastName, string birthDate)
        {
            var user = new User
            {
                UserName = userName,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = DateTime.Parse(birthDate)
            };

            var updatedUser = Connection.Insert<User>(user);

            var queryParams = new List<QueryParameter>()
            {
                new QueryParameter("user_name", ParameterDirection.Input, userName) { ParameterType = typeof(String) }
            };

            var users = Connection.ReadAll<User>(queryParams);

            Assert.NotNull(users);
            Assert.IsType<List<User>>(users);
            Assert.Equal(1, users.Count);
            Assert.Equal(userName, users[0].UserName);
        }

        #endregion POCO Tests

        #region Stored Procedure Tests

        [Theory]
        [MemberData(nameof(NewRecordData))]
        public void TestExecuteStoredProcNonQuery(string userName, string firstName, string lastName, string password, string dateTime)
        {
            try
            {
                var parameters = new List<QueryParameter>()
            {
                new QueryParameter("p_userName", System.Data.ParameterDirection.Input, userName)
                {
                    ParameterType = typeof(System.String)
                },
                new QueryParameter("p_firstName", System.Data.ParameterDirection.Input, firstName)
                {
                    ParameterType = typeof(System.String)
                },
                new QueryParameter("p_lastName", System.Data.ParameterDirection.Input, lastName)
                {
                    ParameterType = typeof(System.String)
                },
                new QueryParameter("p_password", System.Data.ParameterDirection.Input, password)
                {
                    ParameterType = typeof(System.String)
                },
                new QueryParameter("p_date_of_birth", System.Data.ParameterDirection.Input, dateTime)
                {
                    ParameterType = typeof(System.DateTime)
                }
            };

                var dbConnection = Connection;
                dbConnection.ExecuteProcNonQuery("sp_InsertUser", parameters);

                parameters = new List<QueryParameter>()
            {
                new QueryParameter("p_userName", System.Data.ParameterDirection.Input, userName)
                {
                    ParameterType = typeof(System.String)
                }
            };

                var users = dbConnection.ReadAll<User>(ParameterizedSelectUserSql, parameters);
                Assert.Single(users);
                dbConnection.ExecuteSQL(DeleteUserSql, parameters);
                users = dbConnection.ReadAll<User>(ParameterizedSelectUserSql, parameters);
                Assert.Empty(users);
            }
            catch (DataAccessException ex)
            {
                Assert.Equal("SQLite does not support stored procedures", ex.InnerException.Message);
            }
        }

        [Fact]
        public void TestExecuteStoredProcDataSet()
        {
            try
            {
                DataSet dsUsers = Connection.ExecuteProcDataSet("GetUsers", new List<QueryParameter>(), "users");
            }
            catch (DataAccessException ex)
            {
                Assert.Equal("SQLite does not support stored procedures", ex.InnerException.Message);
            }
        }

        [Fact]
        public void TestExecuteStoredProcDataReader()
        {
            try
            {
                var connection = Connection;
                IDataReader userReader = connection.ExecuteProcDataReader("GetUsers", new List<QueryParameter>());

                Assert.NotNull(userReader);
                Assert.Equal(5, userReader.FieldCount);

                Transform<User> entityReader = new Transform<User>(connection, userReader);
                List<User> users = entityReader.ReadAll(userReader);
                Assert.NotEmpty(users);

            }
            catch (DataAccessException ex)
            {
                Assert.Equal("SQLite does not support stored procedures", ex.InnerException.Message);
            }
        }

        [Fact]
        public void TestExecuteStoredProcDataObjects()
        {
            try
            {
                var users = Connection.ExecuteProcDataObjects<User>("GetUsers", new List<QueryParameter>());

                Assert.NotNull(users);
                Assert.IsType<List<User>>(users);
                Assert.NotEmpty(users);
            }
            catch (DataAccessException ex)
            {
                Assert.Equal("SQLite does not support stored procedures", ex.InnerException.Message);
            }
        }

        [Fact]
        public void TestReadMultipleReaders()
        {
            try
            {
                var connection = Connection;
                IDataReader userReader = connection.ExecuteProcDataReader("GetUserMultipleViews", new List<QueryParameter>());
                Assert.NotNull(userReader);
                Assert.Equal(5, userReader.FieldCount);

                Transform<User> entityReader = new Transform<User>(connection, userReader);
                List<User> users = entityReader.ReadAll(userReader);
                Assert.NotEmpty(users);

                userReader.NextResult();
                entityReader = new Transform<User>(connection, userReader);
                List<User> users1 = entityReader.ReadAll(userReader);
                Assert.Single(users1);
            }
            catch (DataAccessException ex)
            {
                Assert.Equal("SQLite does not support stored procedures", ex.InnerException.Message);
            }
        }

        [Fact]
        public void TestReadMultipleReaders2()
        {
            try
            {
                var connection = Connection;
                var allLists = connection.ExecuteProcDataObjects<User, User>("GetUserMultipleViews", new List<QueryParameter>());
                Assert.True(allLists.Count == 2);
                List<User> users = (List<User>)allLists[0];
                Assert.NotEmpty(users);

                List<User> users1 = (List<User>)allLists[1];
                Assert.Single(users1);

            }
            catch (DataAccessException ex)
            {
                Assert.Equal("SQLite does not support stored procedures", ex.InnerException.Message);
            }        }

        #endregion Stored Procedure Tests

        #region SQL Tests

        [Theory]
        [InlineData("raja")]
        public void TestExecuteSQL(string userName)
        {
            var dbConnection = Connection;
            dbConnection.ExecuteSQL(InsertNewUserSql);

            var users = dbConnection.ReadAll<User>(SingleUserSelectSql);
            Assert.Equal(1, users.Count);

            var parameters = new List<QueryParameter>()
            {
                new QueryParameter("p_userName", System.Data.ParameterDirection.Input, userName)
                {
                    ParameterType = typeof(System.String)
                }
            };

            dbConnection.ExecuteSQL(DeleteUserSql, parameters);
            users = dbConnection.ReadAll<User>(ParameterizedSelectUserSql, parameters);
            Assert.Equal(0, users.Count);
        }

        #endregion SQL Tests

        #region Abstract Readonly Properties for Queries

        protected abstract string AllUsersSql { get; }

        protected abstract string AllUsersSqlTableName { get; }

        protected abstract string InsertNewUserSql { get; }

        protected abstract string SingleUserSelectSql { get; }

        protected abstract string DeleteUserSql { get; }

        protected abstract string ParameterizedSelectUserSql { get; }

        #endregion

        public static IEnumerable<object[]> NewRecordData() =>
            new List<object[]>
            {
                new object[] { "rashmu", "Rashmika", "Vinakota", "c", "2005-02-03" },
                new object[] { "raja", "Raja", "Vinakota", "b", "1972-06-08" }
            };

    }
}
