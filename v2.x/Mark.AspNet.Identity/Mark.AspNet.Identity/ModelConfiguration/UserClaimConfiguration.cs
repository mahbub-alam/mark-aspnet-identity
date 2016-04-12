// Written by: MAB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.AspNet.Identity.ModelConfiguration
{
    /// <summary>
    /// Represents user claim entity configuration.
    /// </summary>
    public class UserClaimConfiguration : EntityConfiguration
    {
        /// <summary>
        /// Configure entity.
        /// </summary>
        protected override void Configure()
        {
            TableName = Entities.UserClaim;
            this[UserClaimFields.Id] = UserClaimFields.Id;
            this[UserClaimFields.ClaimType] = UserClaimFields.ClaimType;
            this[UserClaimFields.ClaimValue] = UserClaimFields.ClaimValue;
            this[UserClaimFields.UserId] = UserClaimFields.UserId;
        }
    }
}
