using Buggy.Data.Users;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Buggy.Data.Identity
{
    public class BuggyUserManager : UserManager<User>
    {
        public BuggyUserManager(BuggyContext context)
            : base(new UserStore<User>(context))
        {
            UserLockoutEnabledByDefault = true;
            UserTokenProvider = new TotpSecurityStampBasedTokenProvider<User, string>();
        }
    }
}
