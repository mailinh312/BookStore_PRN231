using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BookStoreClient.ShareApiService
{
    public class ApiService
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        public ApiService()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "https://localhost:5000/api";
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Category/AllCategories");
            string strCate = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<CategoryDto> categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDto>>(strCate, options);
            return categories;
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

        public async Task<List<BookDto>> GetBooks()
        {

            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Book/AllBooks");
            string strBook = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<BookDto> books = System.Text.Json.JsonSerializer.Deserialize<List<BookDto>>(strBook, options);
            return books;
        }
    }
}
