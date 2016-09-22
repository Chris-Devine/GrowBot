using System;
using Newtonsoft.Json;

namespace GrowBot.Website.Models.Membership
{
    public class CustomPrincipalSerializeModel
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty(".expires")]
        public DateTime TokenExpiry { get; set; }

        [JsonProperty("userName")]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
    }
}