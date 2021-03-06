﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public class PlannerClient : ITaskSyncable
    {
        private RestClient restClient = new RestClient();
        private Uri getPlannerTasksRequestUri = new Uri(Environment.GetEnvironmentVariable("logic-get-planner-tasks-url"));
        private Uri updatePlannerTaskRequestUri = new Uri(Environment.GetEnvironmentVariable("logic-update-planner-task-url"));

        public List<SyncTask> Tasks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<List<PlannerTask>> GetTasksAsync()
        {
            string response = await restClient.ApiPostAsync(getPlannerTasksRequestUri, null);
            List<PlannerTask> plannerTasks = JsonSerializer.Deserialize<List<PlannerTask>>(response);
            return plannerTasks;
        }

        public async Task CompleteTaskAsync(PlannerTask plannerTask)
        {
            plannerTask.PercentComplete = 100;
            await restClient.ApiPostAsync(updatePlannerTaskRequestUri, plannerTask);
        }

        public async Task UpdateTaskAsync(PlannerTask plannerTask)
        {
            await restClient.ApiPostAsync(updatePlannerTaskRequestUri, plannerTask);
        }

        public Task CompleteTaskAsync(SyncTask syncTask)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(SyncTask syncTask)
        {
            throw new NotImplementedException();
        }

        public Task<SyncTask> AddTaskAsync(SyncTask syncTask)
        {
            throw new NotImplementedException();
        }
    }
}
