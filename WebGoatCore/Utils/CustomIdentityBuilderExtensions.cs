using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGoatCore.Utils
{
    public static class CustomIdentityBuilderExtensions
    {
        public static IdentityBuilder AddGoatTotpSecurityStampBasedTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var totpProvider = typeof(GoatTotpSecurityStampBasedTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("GoatTotpSecurityStampBasedTokenProvider", totpProvider);
        }
    }
}
