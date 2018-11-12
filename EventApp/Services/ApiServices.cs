using EventApp.Domain;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public class ApiServices
    {
        private readonly HttpClient _client;
        private static readonly string BASEADDRESS = "http://eventprojectapi.azurewebsites.net/api/";

        public ApiServices()
        {
            _client = new HttpClient();
        }

        public async Task<HttpResponseMessage> UserExistsAsync(Users user)
        {
            var json = JsonConvert.SerializeObject(user);
            
            var checkUserUrl = BASEADDRESS + "Users/MyUser";
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await _client.PostAsync(new Uri(checkUserUrl), httpContent);
             
        }

        public async Task<HttpResponseMessage> CreateNewParticipationAsync(Participations participation)
        {
            var json = JsonConvert.SerializeObject(participation);
            var checkUserUrl = BASEADDRESS + "Participations";
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await _client.PostAsync(new Uri(checkUserUrl), httpContent);
            
        }

        public async Task<HttpResponseMessage> CreateNewUserAsync(Users user)
        {
            HttpResponseMessage httpResponseMessage = await UserExistsAsync(user);
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    var json = JsonConvert.SerializeObject(user);
                    var checkUserUrl = BASEADDRESS + "Users/NewUser";
                    HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    return await _client.PostAsync(new Uri(checkUserUrl), httpContent);

                case HttpStatusCode.OK:
                    HttpResponseMessage notFound = new HttpResponseMessage();
                    notFound.StatusCode = HttpStatusCode.NotFound;
                    return notFound;

                default:
                    HttpResponseMessage internalServerError = new HttpResponseMessage();
                    internalServerError.StatusCode = HttpStatusCode.InternalServerError;
                    return internalServerError;

            }

        }
    }
}
