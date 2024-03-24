using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStoreClient.ShareApiService
{
    public class ProductApiService
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        public ProductApiService()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "https://localhost:5000/api";
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

        public async Task<BookDto> GetBookById(int id)
        {

            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Book/" + id);
            string strBook = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            BookDto book = System.Text.Json.JsonSerializer.Deserialize<BookDto>(strBook, options);
            return book;
        }

        public async Task<HttpResponseMessage> CreateBook(BookCreateDto model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(ApiUrl + "/Book/Create", jsonContent);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateBook(BookDto model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(ApiUrl + "/Book/Update", jsonContent);
            return response;
        }
    }
}
