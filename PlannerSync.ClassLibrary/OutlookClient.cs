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
        private Uri getTasksRequestUri = new Uri(Environment.GetEnvironmentVariable("logic-get-outlook-tasks-url"));
        private Uri addTaskRequestUri = new Uri(Environment.GetEnvironmentVariable("logic-add-outlook-task-url"));

        public async Task<List<OutlookTask>> GetTasksAsync()
        {
            string response = await restClient.ApiPostAsync(getTasksRequestUri, null);
            List<OutlookTask> tasks = JsonSerializer.Deserialize<List<OutlookTask>>(response);
            return tasks;
        }

        public async Task<OutlookTask> AddOutlookTaskAsync(OutlookTask outlookTask)
        {
            string response = await restClient.ApiPostAsync(addTaskRequestUri, outlookTask);
            OutlookTask addedOutlookTask = JsonSerializer.Deserialize<OutlookTask>(response);
            return addedOutlookTask;
        }
    }
}
