using BusinessObjects.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStoreClient.ShareApiService
{
    public class UserApiService
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        public UserApiService()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "https://localhost:5000/api";
        }

        public async Task<List<UserDto>> GetUsers()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/AppUser/AllUsers");
            string strUser = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<UserDto> users = System.Text.Json.JsonSerializer.Deserialize<List<UserDto>>(strUser, options);
            return users;
        }

        public async Task<HttpResponseMessage> Login(LoginDTO model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(ApiUrl + "/AppUser/Login", jsonContent);
            return response;
        }

        public async Task<HttpResponseMessage> Register(RegisterDTO model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(ApiUrl + "/AppUser/Register", jsonContent);
            return response;
        }
    }
}
