using Microsoft.AspNetCore.Mvc;
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
    public class HttpProductServiceClient: IProductServiceClient
    {
        private HttpClient httpClient;

        public HttpProductServiceClient(
            IOptions<ProductServiceOptions> serviceOptions)
        {
            var url = serviceOptions.Value.Url;

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
        }

        public HttpStatusCode CreateProduct(Product product)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(String.Format("api/Product/CreateProduct"), stringContent).Result;
            return response.StatusCode;
        }

        public HttpStatusCode DeleteProduct(int id)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync(String.Format("api/Product/DeleteProduct?id={0}",id)).Result;

            return response.StatusCode;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync(String.Format("api/Product/GetAllProducts")).Result;
            string jsonData = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IList<Product>>(jsonData);
            return result;
        }
    }
}

