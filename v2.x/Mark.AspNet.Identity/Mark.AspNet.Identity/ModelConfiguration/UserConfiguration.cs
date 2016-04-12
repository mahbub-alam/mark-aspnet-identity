// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user entity configuration.
    /// </summary>
    public class UserConfiguration : EntityConfiguration
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            TableName = Entities.User;
            this[UserFields.Id] = UserFields.Id;
            this[UserFields.UserName] = UserFields.UserName;
            this[UserFields.PasswordHash] = UserFields.PasswordHash;
            this[UserFields.SecurityStamp] = UserFields.SecurityStamp;
            this[UserFields.Email] = UserFields.Email;
            this[UserFields.EmailConfirmed] = UserFields.EmailConfirmed;
            this[UserFields.PhoneNumber] = UserFields.PhoneNumber;
            this[UserFields.PhoneNumberConfirmed] = UserFields.PhoneNumberConfirmed;
            this[UserFields.TwoFactorEnabled] = UserFields.TwoFactorEnabled;
            this[UserFields.LockoutEnabled] = UserFields.LockoutEnabled;
            this[UserFields.LockoutEndDateUtc] = UserFields.LockoutEndDateUtc;
            this[UserFields.AccessFailedCount] = UserFields.AccessFailedCount;
        }
    }
}
