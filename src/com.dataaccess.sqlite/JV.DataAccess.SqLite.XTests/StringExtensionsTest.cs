﻿using JV.DataAccess.Core.Extensions;
using Xunit;

namespace JV.DataAccess.Core.XTests
{
    public class StringExtensionsTest
    {

        [Fact]
        public void TestEmptyConnectionString()
        {
            string emptyString = string.Empty;

            var parts = emptyString.SplitDataAccessConnectionString();
            Assert.True(parts.Count == 0);
        }

        [Theory]
        [InlineData("provider=JV.DataAccess.Sqlite; provider connection string='abc'")]
        [InlineData("provider=JV.DataAccess.Sqlite; provider connection string='abc';")]
        public void TestSimpleConnectionString(string connString)
        {
            var parts = connString.SplitDataAccessConnectionString();

            Assert.True(parts.Count == 2);
            Assert.True(parts["provider"] == "JV.DataAccess.Sqlite");
            Assert.True(parts["provider connection string"] == "abc");
        }

        [Theory]
        [InlineData("provider=JV.DataAccess.Sqlite; provider connection string='Data Source=localhost;user id=abc;Password=Test1234;';")]
        [InlineData("Provider=JV.DataAccess.Sqlite; PROVIDER CONNECTION STRING='Data Source=localhost;user id=abc;Password=Test1234;';")]
        public void TestMultiPartsConnectionString(string connString)
        {
            var parts = connString.SplitDataAccessConnectionString();

            Assert.True(parts.Count == 2);
            Assert.True(parts["provider"] == "JV.DataAccess.Sqlite");
            Assert.True(parts["provider connection string"] == "Data Source=localhost;user id=abc;Password=Test1234;");
        }

        [Theory]
        [InlineData("provider=JV.DataAccess.Sqlite; provider connection string=;")]
        [InlineData("Provider=; PROVIDER CONNECTION STRING='Data Source=localhost;user id=abc;Password=Test1234;';")]
        public void TestMultiPartsConnectionStringWithEmptyValue(string connString)
        {
            var parts = connString.SplitDataAccessConnectionString();

            Assert.True(parts.Count == 2);
        }

    }
}
