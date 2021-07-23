using Jsits.DataAccess.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsits.DataAccess.Core.Interfaces {

    public interface IQueryBuilder<T> where T : class, new() {

        EntityMapping Map { get; set; }

        string SelectAllStatement { get; }

        string SelectStatement(IList<QueryParameter> queryParameters);

        string InsertStatement { get; }

        string UpdateStatement { get; }

        string DeleteStatement { get; }

    }

}
