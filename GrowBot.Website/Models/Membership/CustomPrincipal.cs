using System.Linq;
using System.Security.Principal;

namespace GrowBot.Website.Models.Membership
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(string email)
        {
            Identity = new GenericIdentity(email);
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => r == role);
        }

        public string Token { get; set; }
        public string TokenExpiry { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}