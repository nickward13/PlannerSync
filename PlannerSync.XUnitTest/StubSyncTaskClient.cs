using PlannerSync.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.XUnitTest
{
    class StubSyncTaskClient : ISyncTaskClient
    {
        public List<SyncTask> Tasks { get; set; }

        public StubSyncTaskClient()
        {
            Tasks = new List<SyncTask>();
        }

        public async Task<SyncTask> AddTaskAsync(SyncTask syncTask)
        {
            SyncTask newSyncTask = new SyncTask();
            newSyncTask.Id = Guid.NewGuid().ToString();
            newSyncTask.Description = syncTask.Description;
            newSyncTask.DueDateTime = syncTask.DueDateTime;
            newSyncTask.Title = syncTask.Title;
            Tasks.Add(newSyncTask);
            return newSyncTask;
        }

        public async Task CompleteTaskAsync(SyncTask syncTask)
        {
            Tasks.Remove(syncTask);
        }

        public async Task UpdateTaskAsync(SyncTask syncTask)
        {
            var taskToUpdate = Tasks.First<SyncTask>(t => t.Id == syncTask.Id);
            taskToUpdate = syncTask;
        }
    }
}
