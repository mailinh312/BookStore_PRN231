using BusinessObjects.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStoreClient.ShareApiService
{
    public class AuthorApiService
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        public AuthorApiService()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "https://localhost:5000/api";
        }

        public async Task<List<AuthorDto>> GetAuthors()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Author/AllAuthors");
            string strAuthor = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<AuthorDto> authors = System.Text.Json.JsonSerializer.Deserialize<List<AuthorDto>>(strAuthor, options);
            return authors;
        }

        public async Task<AuthorDto> GetAuthorById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Author/" + id);
            string strAuthor = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            AuthorDto author = System.Text.Json.JsonSerializer.Deserialize<AuthorDto>(strAuthor, options);
            return author;
        }

        public async Task<HttpResponseMessage> CreateAuthor(AuthorCreateDto model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(ApiUrl + "/Author/Create", jsonContent);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateAuthor(AuthorDto model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(ApiUrl + "/Author/Update", jsonContent);
            return response;
        }
    }
}

