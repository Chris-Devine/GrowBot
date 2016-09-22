using System.Security.Principal;

namespace GrowBot.Website.Models.Membership
{
    internal interface ICustomPrincipal : IPrincipal
    {
        string Token { get; set; }
        string TokenExpiry { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string[] Roles { get; set; }
    }
}