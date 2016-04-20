// Written by: MAB 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Data.Common;
using System.Configuration;
using System.Reflection;
using System.IO;
using Mark.Data.Common;

namespace Mark.AspNet.Identity.MySql.Tests
{
    [SetUpFixture]
    public class Setup
    {
        public static UnitOfWork UnitOfWork
        {
            get;
            private set;
        }

        public Setup()
        {
        }

        [OneTimeSetUp]
        public void Init()
        {
            UnitOfWork = new UnitOfWork(new MySqlStorageContext());
            SeedDatabase();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            UnitOfWork.Dispose();
            UnitOfWork = null;
        }

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private void SeedDatabase()
        {
            string sqlSeedScriptFileName = Path.Combine(AssemblyDirectory, 
                ConfigurationManager.AppSettings["SqlSeedScript"].ToString());
            string script = GetSeedScriptFromFile(sqlSeedScriptFileName);

            if (script == null)
            {
                throw new Exception("Failed to load SQL seed script");
            }

            MySqlStorageContext sContext = UnitOfWork.StorageContext as MySqlStorageContext;
            DbCommand command = sContext.CreateCommand();

            command.CommandText = script;

            DbCommandContext cmdContext = new DbCommandContext(command);

            sContext.Open();

            try
            {
                cmdContext.Execute();
            }
            catch (Exception)
            {
            }
            finally
            {
                cmdContext.Dispose();
                sContext.Close();
            }
        }

        private string GetSeedScriptFromFile(string fileName)
        {
            StreamReader reader = null;
            string script = null;

            try
            {
                reader = new StreamReader(fileName);

                script = reader.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return script;
        }
    }
}
