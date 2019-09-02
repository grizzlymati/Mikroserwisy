using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class HttpOrderServiceClient: IOrderServiceClient
    {
        private HttpClient httpClient;

        public HttpOrderServiceClient(
            IOptions<OrderServiceOptions> serviceOptions)
        {
            var url = serviceOptions.Value.Url;

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
        }

        public HttpStatusCode CreateNewOrder(Order order)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(String.Format("api/Order/CreateNewOrder"), stringContent).Result;
            return response.StatusCode;
        }

        public HttpStatusCode DeleteOrder(int orderId)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync(String.Format("api/Order/DeleteOrder?orderId={0}", orderId)).Result;

            return response.StatusCode;
        }

        public IEnumerable<Order> GetAllOrders(int userId)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync(String.Format("api/Order/GetAllOrders?userId={0}",userId)).Result;
            string jsonData = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IList<Order>>(jsonData);
            return result;
        }
    }
}

