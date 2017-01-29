using System.Net.Http;
using System.Net.Http.Headers;
using UrerGateway.Business.Models;

namespace UrerGateway.Business.RestClients
{
    public class BaseRestClient : HttpClient
    {
        public BaseRestClient()
        {
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public UrerActionResult Get(string path)
        {
            var response = this.GetAsync(path).Result;
            return this.ExecuteRequest(response);
        }

        public UrerActionResult Post(string path, object data)
        {
            var response = this.PostAsJsonAsync(path, data).Result;
            return this.ExecuteRequest(response);
        }

        public UrerActionResult Put(string path, object data)
        {
            var response = this.PutAsJsonAsync(path, data).Result;
            return this.ExecuteRequest(response);
        }

        public UrerActionResult Put(string path)
        {
            var response = this.PutAsJsonAsync(path, (object) null).Result;
            return this.ExecuteRequest(response);
        }

        public UrerActionResult Delete(string path)
        {
            var response = this.DeleteAsync(path).Result;
            return this.ExecuteRequest(response);
        }

        private UrerActionResult ExecuteRequest(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<object>().Result;
                return new UrerActionResult(response.StatusCode, data);
            }

            return new UrerActionResult(response.StatusCode, response.Content.ReadAsStringAsync().Result);
        }
    }
}