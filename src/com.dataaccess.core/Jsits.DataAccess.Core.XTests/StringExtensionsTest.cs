using Gvs.DataAccess.Core.Extensions;
using Xunit;

namespace Gvs.DataAccess.Core.XTests
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
        [InlineData("provider=Gvs.DataAccess.MySql; provider connection string='abc'")]
        [InlineData("provider=Gvs.DataAccess.MySql; provider connection string='abc';")]
        public void TestSimpleConnectionString(string connString)
        {
            var parts = connString.SplitDataAccessConnectionString();

            Assert.True(parts.Count == 2);
            Assert.True(parts["provider"] == "Gvs.DataAccess.MySql");
            Assert.True(parts["provider connection string"] == "abc");
        }

        [Theory]
        [InlineData("provider=Gvs.DataAccess.MySql; provider connection string='data Source=localhost;user id=abc;Password=Test1234;';")]
        [InlineData("Provider=Gvs.DataAccess.MySql; PROVIDER CONNECTION STRING='data Source=localhost;user id=abc;Password=Test1234;';")]
        public void TestMultiPartsConnectionString(string connString)
        {
            var parts = connString.SplitDataAccessConnectionString();

            Assert.True(parts.Count == 2);
            Assert.True(parts["provider"] == "Gvs.DataAccess.MySql");
            Assert.True(parts["provider connection string"] == "data Source=localhost;user id=abc;Password=Test1234;");
        }

        [Theory]
        [InlineData("provider=Gvs.DataAccess.MySql; provider connection string=;")]
        [InlineData("Provider=; PROVIDER CONNECTION STRING='data Source=localhost;user id=abc;Password=Test1234;';")]
        public void TestMultiPartsConnectionStringWithEmptyValue(string connString)
        {
            var parts = connString.SplitDataAccessConnectionString();

            Assert.True(parts.Count == 2);
        }

    }
}
