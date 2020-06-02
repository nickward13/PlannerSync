using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public class OutlookLogicAppClient : ITaskSyncable
    {
        private RestClient restClient = new RestClient();
        private Uri getTasksRequestUri = new Uri(Environment.GetEnvironmentVariable("logic-get-outlook-tasks-url"));
        private Uri addTaskRequestUri = new Uri(Environment.GetEnvironmentVariable("logic-add-outlook-task-url"));
        private Uri updateTaskRequestUri = new Uri(Environment.GetEnvironmentVariable("logic-update-outlook-task-url"));

        public List<SyncTask> Tasks { 
            get 
            {
                List<SyncTask> tasks = new List<SyncTask>();
                foreach(var outlookTask in outlookTasks)
                {
                    SyncTask syncTask = ConvertToSyncTask(outlookTask);
                    tasks.Add(syncTask);
                }
                return tasks;
            }
            set => throw new NotImplementedException(); }

        private static SyncTask ConvertToSyncTask(OutlookTask outlookTask)
        {
            DateTime dueDateTime = DateTime.MinValue;
            if (outlookTask.DueDateTime != null
                && !string.IsNullOrEmpty(outlookTask.DueDateTime.DateTime))
                dueDateTime = DateTime.Parse(outlookTask.DueDateTime.DateTime);
            
            string description = string.Empty;
            if (outlookTask.Body != null)
                description = outlookTask.Body.Content;
            
            SyncTask syncTask = new SyncTask()
            {
                Id = outlookTask.Id,
                Title = outlookTask.Subject,
                Description = description,
                DueDateTime = dueDateTime                
            };
            return syncTask;
        }

        private List<OutlookTask> outlookTasks;

        public async Task RefreshTasksAsync()
        {
            string response = await restClient.ApiPostAsync(getTasksRequestUri, null);
            outlookTasks = JsonSerializer.Deserialize<List<OutlookTask>>(response);
        }

        public async Task<OutlookTask> AddOutlookTaskAsync(OutlookTask outlookTask)
        {
            string response = await restClient.ApiPostAsync(addTaskRequestUri, outlookTask);
            OutlookTask addedOutlookTask = JsonSerializer.Deserialize<OutlookTask>(response);
            return addedOutlookTask;
        }

        public async Task CompleteOutlookTaskAsync(OutlookTask outlookTask)
        {
            outlookTask.Status = "Completed";
            await restClient.ApiPostAsync(updateTaskRequestUri, outlookTask);
        }

        public async Task UpdateOutlookTaskAsync(OutlookTask outlookTask)
        {
            await restClient.ApiPostAsync(updateTaskRequestUri, outlookTask);
        }

        public Task CompleteTaskAsync(SyncTask syncTask)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(SyncTask syncTask)
        {
            throw new NotImplementedException();
        }

        public async Task<SyncTask> AddTaskAsync(SyncTask syncTask)
        {
            OutlookTask outlookTask = new OutlookTask()
            {
                Subject = syncTask.Title
            };
            string response = await restClient.ApiPostAsync(addTaskRequestUri, outlookTask);
            OutlookTask addedOutlookTask = JsonSerializer.Deserialize<OutlookTask>(response);
            SyncTask addedTask = ConvertToSyncTask(addedOutlookTask);
            return addedTask;
        }
    }
}
