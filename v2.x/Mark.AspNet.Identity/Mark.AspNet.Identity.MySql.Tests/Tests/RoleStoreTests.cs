//
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
using Mark.Data.Common;

namespace Mark.AspNet.Identity.MySql.Tests
{
    [TestFixture]
    public class RoleStoreTests
    {
        private UnitOfWork _unitOfWork;
        private ApplicationRoleStore _roleStore;

        [OneTimeSetUp]
        public void Init()
        {
            _unitOfWork = Setup.UnitOfWork;
            _roleStore = new ApplicationRoleStore(_unitOfWork);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _roleStore.Dispose();
            _roleStore = null;
            _unitOfWork = null;
        }

        [Test]
        public async Task When__RoleStore_Find_By_Id__Expect__No_Zero_Id()
        {
            int roleId = 0;
            ApplicationRole role = await _roleStore.FindByIdAsync(roleId);

            Assert.That(role, Is.Null, "Role is not found");
        }

        [Test]
        public async Task When__RoleStore_Find_By_Id__Expect__Role_Returned()
        {
            int roleId = 1;
            ApplicationRole role = await _roleStore.FindByIdAsync(roleId);

            Assert.That(role, Is.Not.Null, "Role is not found");
            Assert.That(role.Name, Is.EqualTo("System Administrator"), "Wrong role");
        }

        [Test]
        public async Task When__RoleStore_Find_By_Name__Expect__Null_Returned()
        {
            ApplicationRole role = await _roleStore.FindByNameAsync(null).ConfigureAwait(false);
            Assert.That(role, Is.Null, "role not null");
        }

        [Test]
        public async Task When__RoleStore_Find_By_Name__Expect__Role_Returned()
        {
            string roleName = "System Administrator";
            ApplicationRole role = await _roleStore.FindByNameAsync(roleName).ConfigureAwait(false);

            Assert.That(role, Is.Not.Null, "Role is not found");
            Assert.That(role.Name, Is.EqualTo(roleName), "Wrong role");
        }

        [Test]
        public void When__RoleStore_Create_Role__Expcect__Role_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _roleStore.CreateAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__RoleStore_Create_Role__Expect__Role_Created()
        {
            ApplicationRole role = new ApplicationRole();
            role.Name = "User";

            await _roleStore.CreateAsync(role).ConfigureAwait(false);

            ApplicationRole savedRole = await _roleStore.FindByNameAsync(role.Name).ConfigureAwait(false);

            Assert.That(savedRole, Is.Not.Null, "Role is not created");

            Assert.That(savedRole.Name, Is.EqualTo(role.Name), "Wrong role ");
        }

        [Test]
        public void When__RoleStore_Update_Role__Expcect__Role_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _roleStore.UpdateAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__RoleStore_Update_Role__Expect_Role_Updated()
        {
            ApplicationRole role = new ApplicationRole();
            role.Id = 2;

            role = await _roleStore.FindByIdAsync(role.Id).ConfigureAwait(false);

            Assert.That(role, Is.Not.Null, "Role not found for update");

            role.Name = "Application Administrator";

            await _roleStore.UpdateAsync(role).ConfigureAwait(false);

            ApplicationRole savedRole = 
                await _roleStore.FindByNameAsync(role.Name).ConfigureAwait(false);

            Assert.That(savedRole, Is.Not.Null, "Role is not updated");
        }

        [Test]
        public void When__RoleStore_Delete_Role__Expcect__Role_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _roleStore.DeleteAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__RoleStore_Delete_Role__Expect__Role_Deleted()
        {
            ApplicationRole role = null;
            int roleId = 5;

            role = await _roleStore.FindByIdAsync(roleId).ConfigureAwait(false);

            Assert.That(role, Is.Not.Null, "Role not found");

            await _roleStore.DeleteAsync(role).ConfigureAwait(false);

            ApplicationRole savedRole = await 
                _roleStore.FindByIdAsync(roleId).ConfigureAwait(false);

            Assert.That(savedRole, Is.Null, "Role is not deleted");
        }
    }
}
