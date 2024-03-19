using BusinessObjects.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStoreClient.ShareApiService
{
    public class OrderApiService
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        public OrderApiService()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "https://localhost:5000/api";
        }
        public async Task<List<OrderDto>> GetOrders()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Order/AllOrders");
            string strOrder = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<OrderDto> orders = System.Text.Json.JsonSerializer.Deserialize<List<OrderDto>>(strOrder, options);
            return orders;
        }

        public async Task<HttpResponseMessage> UpdateOrderStatus(int orderId, int statusId)
        {
            var requestData = new
            {
                statusId = statusId,
                orderId = orderId
                
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(ApiUrl + "/Order/Update?orderId=" + orderId + "&statusId=" + statusId, jsonContent);
            return response;
        }

        public async Task<List<OrderDetailDto>> GetOrderDetailByOrderId(int orderId)
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/OrderDetail/OrderId?id=" +orderId);
            string strdetail = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<OrderDetailDto> detail = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetailDto>>(strdetail, options);
            return detail;
        }

        public async Task<List<StatusDto>> GetAllStatus()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl + "/Order/AllStatus");
            string strStatus = await response.Content.ReadAsStringAsync();


            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<StatusDto> status = System.Text.Json.JsonSerializer.Deserialize<List<StatusDto>>(strStatus, options);
            return status;
        }
    }
}
