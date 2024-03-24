using BusinessObjects.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStoreClient.ShareApiService
{
    public class CategoryApiService
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        public CategoryApiService()
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

        public async Task<List<Top3Category>> GetTop3Categories()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Category/Top3Categories");
            string strCate = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Top3Category> categories = System.Text.Json.JsonSerializer.Deserialize<List<Top3Category>>(strCate, options);
            return categories;
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Category/" + id);
            string strCate = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            CategoryDto category = System.Text.Json.JsonSerializer.Deserialize<CategoryDto>(strCate, options);
            return category;
        }

        public async Task<HttpResponseMessage> CreateCategory(CategoryCreateDto model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ApiUrl + "/Category/Create", jsonContent);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateCategory(CategoryDto model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(ApiUrl + "/Category/Update", jsonContent);
            return response;
        }
    }
}
