using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public class OutlookClient
    {
        private RestClient restClient = new RestClient();

        public async Task<List<OutlookTask>> GetTasksAsync()
        {
            Uri requestUri = new Uri(Environment.GetEnvironmentVariable("logic-get-outlook-tasks-url"));
            string response = await restClient.ApiPostAsync(requestUri, null);
            List<OutlookTask> tasks = JsonSerializer.Deserialize<List<OutlookTask>>(response);
            return tasks;
        }
    }
}
