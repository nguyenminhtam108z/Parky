using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;
using System.Text;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;
        public Repository(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        public async Task<bool> CreateAsync(string url, T objectToCreate, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objectToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToCreate), Encoding.UTF8, "application/json");
            }
            else
                return false;
            var client = _clientFactory.CreateClient();
            if(token != null && token.Length !=0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string url, int id, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+id);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+id);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, T objectToUpdate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objectToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToUpdate), Encoding.UTF8, "application/json");
            }
            else
                return false;
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }
    }
}
