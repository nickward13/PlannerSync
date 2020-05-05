using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public class PlannerClient
    {
        private RestClient restClient = new RestClient();

        public async Task<List<PlannerTask>> GetTasksAsync()
        {
            Uri requestUri = new Uri(Environment.GetEnvironmentVariable("logic-get-planner-tasks-url"));
            string response = await restClient.ApiPostAsync(requestUri, null);
            List<PlannerTask> plannerTasks = JsonSerializer.Deserialize<List<PlannerTask>>(response);
            return plannerTasks;
        }

        

    }
}
