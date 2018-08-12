using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoomBookingBot.Extensions
{
    public static class AzureAdExtensions
    {
        public static string GetUserConsentLoginUrl(string tenant, string clientId, string redirectUri, string permissionsRequested, string state)
        {
            return $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize?client_id={clientId}&scope={permissionsRequested}&response_type=code&response_mode=query&redirect_uri={redirectUri}&state={state}";
        }

        public static async Task<string> GetAzureAdToken(this HttpClient client, string tenant, string code, string clientId, string redirectUri, string clientSecret, string permissionsRequested)
        {
            var formFields = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", permissionsRequested),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            };
            var aadAccessTokenRequest = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token") { Content = new FormUrlEncodedContent(formFields) };
            var aadAccessTokenResponse = await client.SendAsync(aadAccessTokenRequest);
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(await aadAccessTokenResponse.Content.ReadAsStringAsync());
            return result.access_token;
        }
    }
}
