// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user login entity configuration.
    /// </summary>
    public class UserLoginConfiguration : EntityConfiguration
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            TableName = Entities.UserLogin;
            this[UserLoginFields.LoginProvider] = UserLoginFields.LoginProvider;
            this[UserLoginFields.ProviderKey] = UserLoginFields.ProviderKey;
            this[UserLoginFields.UserId] = UserLoginFields.UserId;
        }
    }
}
