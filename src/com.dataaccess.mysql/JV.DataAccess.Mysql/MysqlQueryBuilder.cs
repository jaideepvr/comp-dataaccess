﻿using JV.DataAccess.Core.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.DataAccess.Mysql {

    public class MysqlQueryBuilder<T> : QueryBuilder<T> where T: class, new() {

    }

}
