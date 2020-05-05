using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    internal class RestClient
    {
        private HttpClient httpClient = new HttpClient();

        internal async Task<string> ApiPostAsync(Uri requestUri, object body)
        {
            string jsonBody;
            StringContent content;

            if (body != null)
            {
                jsonBody = JsonSerializer.Serialize(body);
                content = new StringContent(
                    jsonBody,
                    Encoding.UTF8,
                    "application/json");
            }
            else
            {
                content = new StringContent(
                    "",
                    Encoding.UTF8,
                    "application/json");
            }

            HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

    }
}
