using System.Security.Claims;
using System.Web;
using GrowBot.Constants;
using GrowBot.Website.Models.Membership;
using RestSharp;

namespace GrowBot.Website.Classes.Helpers
{
    public class HttpHelper
    {
        public enum ClientRequestType
        {
            Delete,
            Get,
            Post,
            Put,
        }

        private RestClient GetRestClient()
        {
            string baseUrl = GrowBotConstants.GrowBotAPI;

            var client = new RestClient(baseUrl);

            return client;
        }

        public IRestResponse ExecuteRequest(RestRequest restRequest)
        {
            RestClient client = GetRestClient();

            CheckAndPossiblyRefreshToken((HttpContext.Current.User.Identity as ClaimsIdentity));

            var user = (HttpContext.Current.User as CustomPrincipal);
            if (user != null && user.Token != null)
            {
                string token = user.Token;
                restRequest.AddParameter(
                    "Authorization",
                    string.Format("Bearer {0}", token), ParameterType.HttpHeader);
            }

            IRestResponse response = client.Execute(restRequest);

            return response;
            ;
        }

        private static async void CheckAndPossiblyRefreshToken(ClaimsIdentity id)
        {
        }

        private Method GetRestClientType(ClientRequestType clientRequestType)
        {
            switch (clientRequestType)
            {
                case ClientRequestType.Delete:
                    return Method.DELETE;
                    break;
                case ClientRequestType.Post:
                    return Method.POST;
                    break;
                case ClientRequestType.Get:
                    return Method.GET;
                    break;
                case ClientRequestType.Put:
                    return Method.PUT;
                    break;
                default:
                    return Method.GET;
            }
        }
    }
}