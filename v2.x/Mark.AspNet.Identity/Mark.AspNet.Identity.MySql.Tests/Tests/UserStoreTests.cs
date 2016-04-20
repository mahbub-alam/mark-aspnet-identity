// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using NUnit.Framework;
using Microsoft.AspNet.Identity;
using Mark.Data.Common;

namespace Mark.AspNet.Identity.MySql.Tests
{
    [TestFixture]
    public class UserStoreTests
    {
        private UnitOfWork _unitOfWork;
        private ApplicationUserStore _userStore;

        [OneTimeSetUp]
        public void Init()
        {
            _unitOfWork = Setup.UnitOfWork;
            _userStore = new ApplicationUserStore(_unitOfWork);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _userStore.Dispose();
            _userStore = null;
            _unitOfWork = null;
        }


        [Test]
        public void When__UserStore_Add_Claim__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                Claim claim = new Claim("Delete Log", "Yes");

                await _userStore.AddClaimAsync(null, claim).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public void When__UserStore_Add_Claim__Expect__Claim_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = 1;

                await _userStore.AddClaimAsync(user, null).ConfigureAwait(false);
            }, "Exception not thrown");
        }


        [Test]
        public async Task When__UserStore_Add_Claim__Expect__Claim_Added()
        {
            Claim claim = new Claim("Delete Log", "Yes");
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            Assert.That(user, Is.Not.Null, "User not found for claim");

            await _userStore.AddClaimAsync(user, claim).ConfigureAwait(false);

            _unitOfWork.SaveChanges();

            IList<Claim> claims = await _userStore.GetClaimsAsync(user).ConfigureAwait(false);

            Claim savedClaim = claims.Where(p => 
                p.Type == claim.Type && 
                p.Value == claim.Value).FirstOrDefault();

            Assert.That(savedClaim, Is.Not.Null, "Claim not added");
        }

        [Test]
        public void When__UserStore_Add_Login__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                UserLoginInfo login = new UserLoginInfo("Google", "E5A1F645-088A-4984-A7CE-CA1D48D30F92");

                await _userStore.AddLoginAsync(null, login).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public void When__UserStore_Add_Login__Expect__LoginInfo_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = 1;

                await _userStore.AddLoginAsync(user, null).ConfigureAwait(false);
            }, "Exception not thrown");
        }


        [Test]
        public async Task When__UserStore_Add_Login__Expect__Login_Added()
        {
            UserLoginInfo login = new UserLoginInfo("Google", "E5A1F645-088A-4984-A7CE-CA1D48D30F92");
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            await _userStore.AddLoginAsync(user, login).ConfigureAwait(false);

            _unitOfWork.SaveChanges();

            IList<UserLoginInfo> logins = await _userStore.GetLoginsAsync(user).ConfigureAwait(false);

            UserLoginInfo savedLogin = logins.Where(p => 
                p.LoginProvider == login.LoginProvider && 
                p.ProviderKey == login.ProviderKey).FirstOrDefault();

            Assert.That(savedLogin, Is.Not.Null, "Login not added");
        }

        [Test]
        public void When__UserStore_Add_To_Role__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                string roleName = "System Administrator";

                await _userStore.AddToRoleAsync(null, roleName).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public void When__UserStore_Add_To_Role__Expect__RoleName_Param_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentException), async () =>
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = 1;

                await _userStore.AddToRoleAsync(user, " ").ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Add_To_Role__Expect__User_Added_To_Role()
        {
            string roleName = "Report User";
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            await _userStore.AddToRoleAsync(user, roleName).ConfigureAwait(false);

            _unitOfWork.SaveChanges();

            IList<string> roles = await _userStore.GetRolesAsync(user).ConfigureAwait(false);

            string savedRole = roles.Where(p => p == roleName).FirstOrDefault();

            Assert.That(savedRole, Is.Not.Null, "User not added to role");
        }

        [Test]
        public void When__UserStore_Create_User__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.CreateAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Create_User__Expect__User_Created()
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "alice";

            await _userStore.CreateAsync(user).ConfigureAwait(false);

            ApplicationUser savedUser = await _userStore.FindByNameAsync(user.UserName).ConfigureAwait(false);

            Assert.That(savedUser, Is.Not.Null, "User is not created");

            Assert.That(savedUser.UserName, Is.EqualTo(user.UserName), "Wrong user ");
        }

        [Test]
        public void When__UserStore_Delete_User__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.DeleteAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Delete_User__Expect__User_Deleted()
        {
            ApplicationUser user = null;
            int userId = 3;

            user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            Assert.That(user, Is.Not.Null, "User not found");

            await _userStore.DeleteAsync(user).ConfigureAwait(false);

            ApplicationUser savedUser = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            Assert.That(savedUser, Is.Null, "User is not deleted");
        }

        [Test]
        public void When__UserStore_Find_By_LoginInfo__Expect__LoginInfo_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                ApplicationUser user = await _userStore.FindAsync(null).ConfigureAwait(false);
            });
        }

        [Test]
        public async Task When__UserStore_Find_By_LoginInfo__Expect__User_Returned()
        {
            UserLoginInfo login = new UserLoginInfo("Microsoft", "A516261B-0AE5-4D7F-926F-910C1A2BE51A");

            ApplicationUser user = await _userStore.FindAsync(login).ConfigureAwait(false);

            Assert.That(user, Is.Not.Null, "User not found");
            Assert.That(user.UserName, Is.EqualTo("john_doe"), "Wrong user");
        }

        [Test]
        public void When__UserStore_Find_By_Email__Expect__Email_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentException), async () =>
            {
                ApplicationUser user = await _userStore.FindByEmailAsync(" ").ConfigureAwait(false);
            });
        }

        [Test]
        public async Task When__UserStore_Find_By_Email__Expect__User_Returned()
        {
            string email = "john_doe@exmaple.com";

            ApplicationUser user = await _userStore.FindByEmailAsync(email).ConfigureAwait(false);

            Assert.That(user, Is.Not.Null, "User not found");
            Assert.That(user.UserName, Is.EqualTo("john_doe"), "Wrong user");
        }

        [Test]
        public async Task When__UserStore_Find_By_Id__Expect__No_Zero_Id()
        {
            int userId = 0;
            ApplicationUser user = await _userStore.FindByIdAsync(userId);

            Assert.That(user, Is.Null, "User is not found");
        }

        [Test]
        public async Task When__UserStore_Find_By_Id__Expect__User_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId);

            Assert.That(user, Is.Not.Null, "User is not found");
            Assert.That(user.UserName, Is.EqualTo("john_doe"), "Wrong user");
        }

        [Test]
        public void When__UserStore_Get_Find_By_Name__Expect__UserName_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentException), async () =>
            {
                ApplicationUser user = await _userStore.FindByNameAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Find_By_Name__Expect__User_Returned()
        {
            string userName = "john_doe";
            ApplicationUser user = await _userStore.FindByNameAsync(userName).ConfigureAwait(false);

            Assert.That(user, Is.Not.Null, "User is not found");
            Assert.That(user.UserName, Is.EqualTo(userName), "Wrong user");
        }

        [Test]
        public void When__UserStore_Get_Access_Failed_Count__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                int value = await _userStore.GetAccessFailedCountAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Access_Failed_Count__Expect__Count_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            int value = await _userStore.GetAccessFailedCountAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.Not.Zero, "AccessFailedCount is zero");
        }

        [Test]
        public void When__UserStore_Get_Claims__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetClaimsAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Claims__Expect__Claims_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            IList<Claim> claims = await _userStore.GetClaimsAsync(user).ConfigureAwait(false);

            Assert.That(claims.Count, Is.Not.Zero, "Claims not found");
        }

        [Test]
        public void When__UserStore_Get_Email__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetEmailAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Email__Expect__Email_Returned()
        {
            int userId = 1;
            string expectedValue = "john_doe@exmaple.com";
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = await _userStore.GetEmailAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.EqualTo(expectedValue), "User email not found");
        }

        [Test]
        public void When__UserStore_Get_Email_Confirmed__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetEmailConfirmedAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Email_Confirmed__Expect__Email_Confirmed_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = await _userStore.GetEmailConfirmedAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.True, "User email not confirmed");
        }

        [Test]
        public void When__UserStore_Get_Lockout_Enabled__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetLockoutEnabledAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Lockout_Enabled__Expect__Lockout_Enabled_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = await _userStore.GetLockoutEnabledAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.True, "User lockout not enabled");
        }

        [Test]
        public void When__UserStore_Get_Lockout_End_Date__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetLockoutEndDateAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Lockout_End_Date__Expect__Lockout_End_Date_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            DateTimeOffset value = await _userStore.GetLockoutEndDateAsync(user).ConfigureAwait(false);
            DateTimeOffset expectedValue = new DateTimeOffset(DateTime.SpecifyKind(
                new DateTime(2016, 4, 20, 0, 0, 0), DateTimeKind.Utc));

            Assert.That(value, Is.EqualTo(expectedValue), "User lockout end date does not match");
        }

        [Test]
        public void When__UserStore_Get_Logins__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetLoginsAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Logins__Expect__Logins_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            IList<UserLoginInfo> logins = await _userStore.GetLoginsAsync(user).ConfigureAwait(false);

            Assert.That(logins.Count, Is.Not.Zero, "Logins not found");
        }

        [Test]
        public void When__UserStore_Get_Password_Hash__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetPasswordHashAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Password_Hash__Expect__Password_Hash_Returned()
        {
            int userId = 1;
            string expectedValue = "10b8b801768fc884a43f5f319313dd8e";
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = await _userStore.GetPasswordHashAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.EqualTo(expectedValue), "User password has is not the same");
        }

        [Test]
        public void When__UserStore_Get_PhoneNumber__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetPhoneNumberAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_PhoneNumber__Expect__PhoneNumber_Returned()
        {
            int userId = 1;
            string expectedValue = "+12142521234";
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = await _userStore.GetPhoneNumberAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.EqualTo(expectedValue), "User phone number not found");
        }

        [Test]
        public void When__UserStore_Get_PhoneNumber_Confirmed__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetPhoneNumberConfirmedAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_PhoneNumber_Confirmed__Expect__PhoneNumber_Confirmed_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = await _userStore.GetPhoneNumberConfirmedAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.True, "User phone number not confirmed");
        }

        [Test]
        public void When__UserStore_Get_Roles__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetRolesAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_Roles__Expect__Roles_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            IList<string> roles = await _userStore.GetRolesAsync(user).ConfigureAwait(false);

            Assert.That(roles.Count, Is.Not.Zero, "User roles not found");
        }

        [Test]
        public void When__UserStore_Get_SecurityStamp__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetSecurityStampAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_SecurityStamp__Expect__SecurityStamp_Returned()
        {
            int userId = 1;
            string expectedValue = "fake_security_stamp";
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = await _userStore.GetSecurityStampAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.EqualTo(expectedValue), "User phone number not found");
        }

        [Test]
        public void When__UserStore_Get_TwoFactor_Enabled__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.GetTwoFactorEnabledAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Get_TwoFactor_Enabled__Expect__TwoFactor_Enabled_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = await _userStore.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.False, "User two factor enabled");
        }

        [Test]
        public void When__UserStore_Has_Password__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.HasPasswordAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Has_Password__Expect__Has_Password_Returned()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = await _userStore.HasPasswordAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.True, "User password not set");
        }

        [Test]
        public void When__UserStore_Increment_Access_Failed_Count__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.IncrementAccessFailedCountAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Increment_Access_Failed_Count__Expect__Count_Returned()
        {
            int userId = 1;
            int expectedValue = 3;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            int value = await _userStore.IncrementAccessFailedCountAsync(user).ConfigureAwait(false);

            Assert.That(value, Is.EqualTo(expectedValue), "User password not set");
        }

        [Test]
        public void When__UserStore_Is_In_Role__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                string roleName = "System Administrator";

                await _userStore.IsInRoleAsync(null, roleName).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public void When__UserStore_Is_In_Role__Expect__RoleName_Param_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentException), async () =>
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = 1;

                await _userStore.IsInRoleAsync(user, " ").ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Is_In_Role__Expect__User_In_Role()
        {
            string roleName = "System Administrator";
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = await _userStore.IsInRoleAsync(user, roleName).ConfigureAwait(false);

            Assert.That(value, Is.True, "User not in role");
        }

        [Test]
        public void When__UserStore_Remove_Claim__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                Claim claim = new Claim("Delete All", "Yes");

                await _userStore.RemoveClaimAsync(null, claim).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public void When__UserStore_Remove_Claim__Expect__Claim_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = 1;

                await _userStore.RemoveClaimAsync(user, null).ConfigureAwait(false);
            }, "Exception not thrown");
        }


        [Test]
        public async Task When__UserStore_Remove_Claim__Expect__Claim_Removed()
        {
            Claim claim = new Claim("Delete All", "Yes");
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            await _userStore.RemoveClaimAsync(user, claim).ConfigureAwait(false);

            _unitOfWork.SaveChanges();

            IList<Claim> claims = await _userStore.GetClaimsAsync(user).ConfigureAwait(false);

            Claim removedClaim = claims.Where(p =>
                p.Type == claim.Type &&
                p.Value == claim.Value).FirstOrDefault();

            Assert.That(removedClaim, Is.Null, "Claim not removed");
        }

        [Test]
        public void When__UserStore_Remove_From_Role__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                string roleName = "Test User";

                await _userStore.RemoveFromRoleAsync(null, roleName).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public void When__UserStore_Remove_From_Role__Expect__RoleName_Param_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentException), async () =>
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = 1;

                await _userStore.RemoveFromRoleAsync(user, " ").ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Remove_From_Role__Expect__User_Role_Removed()
        {
            string roleName = "Test User";
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            await _userStore.RemoveFromRoleAsync(user, roleName).ConfigureAwait(false);

            _unitOfWork.SaveChanges();

            IList<string> roles = await _userStore.GetRolesAsync(user).ConfigureAwait(false);

            string removedRole = roles.Where(p => p == roleName).FirstOrDefault();

            Assert.That(removedRole, Is.Null, "User role not removed");
        }


        [Test]
        public void When__UserStore_Remove_Login__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                UserLoginInfo login = new UserLoginInfo("Yahoo", "341113DF-9091-4750-A2DD-9A1B56A8FCDC");

                await _userStore.RemoveLoginAsync(null, login).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public void When__UserStore_Remove_Login__Expect__LoginInfo_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = 1;

                await _userStore.RemoveLoginAsync(user, null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Remove_Login__Expect__Login_Removed()
        {
            UserLoginInfo login = new UserLoginInfo("Yahoo", "341113DF-9091-4750-A2DD-9A1B56A8FCDC");
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            await _userStore.RemoveLoginAsync(user, login).ConfigureAwait(false);

            _unitOfWork.SaveChanges();

            IList<UserLoginInfo> logins = await _userStore.GetLoginsAsync(user).ConfigureAwait(false);

            UserLoginInfo removedLogin = logins.Where(p =>
                p.LoginProvider == login.LoginProvider &&
                p.ProviderKey == login.ProviderKey).FirstOrDefault();

            Assert.That(removedLogin, Is.Null, "Login not removed");
        }

        [Test]
        public void When__UserStore_Reset_Access_Failed_Count__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.ResetAccessFailedCountAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Reset_Access_Failed_Count__Expect__Count_Reset()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);

            await _userStore.ResetAccessFailedCountAsync(user).ConfigureAwait(false);

            Assert.That(user.AccessFailedCount, Is.Zero, "User access failed count not reset");
        }

        [Test]
        public void When__UserStore_Set_Email__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetEmailAsync(null, "").ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_Email__Expect__Email_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = "john@doe.com";
            await _userStore.SetEmailAsync(user, value).ConfigureAwait(false);

            Assert.That(user.Email, Is.EqualTo(value), "User email not set");
        }

        [Test]
        public void When__UserStore_Set_Email_Confirmed__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetEmailConfirmedAsync(null, false).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_Email_Confirmed__Expect__Email_Confirmed_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = false;
            await _userStore.SetEmailConfirmedAsync(user, value).ConfigureAwait(false);

            Assert.That(user.EmailConfirmed, Is.EqualTo(value), "User email confirmed not set");
        }

        [Test]
        public void When__UserStore_Set_Lockout_Enabled__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetLockoutEnabledAsync(null, false).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_Lockout_Enabled__Expect__Lockout_Enabled_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = false;
            await _userStore.SetLockoutEnabledAsync(user, value).ConfigureAwait(false);

            Assert.That(user.LockoutEnabled, Is.EqualTo(value), "User lockout enabled not set");
        }

        [Test]
        public void When__UserStore_Set_Lockout_End_Date__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetLockoutEndDateAsync(null, new DateTimeOffset()).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_Lockout_End_Date__Expect__Lockout_End_Date_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            DateTimeOffset value = new DateTimeOffset();
            await _userStore.SetLockoutEndDateAsync(user, value).ConfigureAwait(false);

            // Passed default value; so, the property is set null
            Assert.That(user.LockoutEndDateUtc, Is.Null, "User lockout end date not set");
        }

        [Test]
        public void When__UserStore_Set_Password_Hash__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetPasswordHashAsync(null, "").ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_Password_Hash__Expect__Password_Hash_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = "";
            await _userStore.SetPasswordHashAsync(user, value).ConfigureAwait(false);

            Assert.That(user.PasswordHash, Is.EqualTo(value), "User password hash not set");
        }

        [Test]
        public void When__UserStore_Set_PhoneNumber__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetPhoneNumberAsync(null, "").ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_PhoneNumber__Expect__PhoneNumber_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = "";
            await _userStore.SetPhoneNumberAsync(user, value).ConfigureAwait(false);

            Assert.That(user.PhoneNumber, Is.EqualTo(value), "User phone number not set");
        }

        [Test]
        public void When__UserStore_Set_PhoneNumber_Confirmed__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetPhoneNumberConfirmedAsync(null, false).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_PhoneNumber_Confirmed__Expect__PhoneNumber_Confirmed_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = false;
            await _userStore.SetPhoneNumberConfirmedAsync(user, value).ConfigureAwait(false);

            Assert.That(user.PhoneNumberConfirmed, Is.EqualTo(value), "User phone number confirmed not set");
        }

        [Test]
        public void When__UserStore_Set_SecurityStamp__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetSecurityStampAsync(null, "").ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_SecurityStamp__Expect__SecurityStamp_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            string value = "";
            await _userStore.SetSecurityStampAsync(user, value).ConfigureAwait(false);

            Assert.That(user.SecurityStamp, Is.EqualTo(value), "User security stamp not set");
        }

        [Test]
        public void When__UserStore_Set_TwoFactor_Enabled__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.SetTwoFactorEnabledAsync(null, false).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Set_TwoFactor_Enabled__Expect__TwoFactor_Enabled_Set()
        {
            int userId = 1;
            ApplicationUser user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);
            bool value = true;
            await _userStore.SetTwoFactorEnabledAsync(user, value).ConfigureAwait(false);

            Assert.That(user.TwoFactorEnabled, Is.EqualTo(value), "User two factor enabled not set");
        }

        [Test]
        public void When__UserStore_Update_User__Expect__User_Param_Null_Exception()
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), async () =>
            {
                await _userStore.UpdateAsync(null).ConfigureAwait(false);
            }, "Exception not thrown");
        }

        [Test]
        public async Task When__UserStore_Update_User__Expect__User_Updated()
        {
            ApplicationUser user = new ApplicationUser();
            user.Id = 2;

            user = await _userStore.FindByIdAsync(user.Id).ConfigureAwait(false);

            Assert.That(user, Is.Not.Null, "User not found for update");

            user.UserName = "james_bond";

            await _userStore.UpdateAsync(user).ConfigureAwait(false);

            ApplicationUser savedUser =
                await _userStore.FindByNameAsync(user.UserName).ConfigureAwait(false);

            Assert.That(savedUser, Is.Not.Null, "User is not updated");
        }
    }
}
