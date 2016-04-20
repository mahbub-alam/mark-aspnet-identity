// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Data.Common;
using MySql.Data.MySqlClient;

namespace Mark.AspNet.Identity.MySql.Tests
{
    public class MySqlStorageContext : DbStorageContext<MySqlConnection>
    {
        public MySqlStorageContext() : base("DbConnectionString")
        {
        }
    }
}
