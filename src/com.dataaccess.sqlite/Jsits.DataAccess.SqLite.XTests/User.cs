using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsits.DataAccess.Core.XTests.Objects
{
    [Table("user", "user_id")]
    public class User
    {

        public User() { }

        [Field("user_id", true)]
        public int UserId { get; set; }

        [Field("user_name", true)]
        public string UserName { get; set; }

        [Field("password", true)]
        public string Password { get; set; }

        [Field("first_name", true)]
        public string FirstName { get; set; }

        [Field("last_name", true)]
        public string LastName { get; set; }

        [Field("date_of_birth", true)]
        public DateTime BirthDate { get; set; }

    }
}
