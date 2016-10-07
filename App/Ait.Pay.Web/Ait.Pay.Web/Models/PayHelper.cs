using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ait.Pay.Web.Models
{
    static class PayHelper
    {
        public static async Task<R> PostAsJson<R, T>(this string url, T criteria)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync(url, criteria);
                response.EnsureSuccessStatusCode();                
                return await response.Content.ReadAsAsync<R>();
            }
        }


        public static string AsJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}