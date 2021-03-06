﻿//
// Copyright 2016, Mahbub Alam (mahbub002@ymail.com)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//


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
using Mark.DotNet.Data.Common;

namespace Mark.AspNet.Identity.SqlServer.Tests
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
            UnitOfWork = new UnitOfWork(new SqlStorageContext());
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

            SqlStorageContext sContext = UnitOfWork.StorageContext as SqlStorageContext;
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
