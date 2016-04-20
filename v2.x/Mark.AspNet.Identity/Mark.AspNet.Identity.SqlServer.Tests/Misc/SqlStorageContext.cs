// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Data.Common;
using System.Data.SqlClient;

namespace Mark.AspNet.Identity.SqlServer.Tests
{
    public class SqlStorageContext : DbStorageContext<SqlConnection>
    {
        public SqlStorageContext() : base("DbConnectionString")
        {
        }
    }
}
