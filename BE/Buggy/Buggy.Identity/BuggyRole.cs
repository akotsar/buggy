using Microsoft.AspNet.Identity.EntityFramework;

namespace Buggy.Identity
{
    public class BuggyRole : IdentityRole
    {
        public BuggyRole()
        {
        }

        public BuggyRole(string name)
            : base(name)
        {
        }
    }
}
