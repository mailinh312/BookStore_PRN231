using BusinessObjects.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStoreClient.ShareApiService
{
    public class ImportApiService
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        public ImportApiService()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "https://localhost:5000/api";
        }

        public async Task<List<ImportDto>> GetImports()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Import/AllImports");
            string strImport = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<ImportDto> imports = System.Text.Json.JsonSerializer.Deserialize<List<ImportDto>>(strImport, options);
            return imports;
        }

        public async Task<List<ImportDetailDto>> GetImportDetailByImportId(int importId)
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/ImportDetail/Import?id="+importId);
            string strImport = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<ImportDetailDto> importDetails = System.Text.Json.JsonSerializer.Deserialize<List<ImportDetailDto>>(strImport, options);
            return importDetails;
        }
        public async Task<int> AddImport(ImportCreateDto model)
        {

            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(ApiUrl + "/Import/Create", jsonContent);
            string strJson = await response.Content.ReadAsStringAsync();

            dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(strJson);
            int importId = Convert.ToInt32(responseObject);
            return importId;
        }

        public async Task AddImportDetail(int importId, ImportDetailCreateDto model)
        {

            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            await client.PostAsync(ApiUrl + "/ImportDetail/Create/importId=" + importId, jsonContent);
        }
    }
}
