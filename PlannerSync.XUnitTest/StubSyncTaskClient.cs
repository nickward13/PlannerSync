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
            syncTask.Id = Guid.NewGuid().ToString();
            Tasks.Add(syncTask);
            return syncTask;
        }

        public async Task CompleteTaskAsync(SyncTask syncTask)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTaskAsync(SyncTask syncTask)
        {
            var taskToUpdate = Tasks.First<SyncTask>(t => t.Id == syncTask.Id);
            taskToUpdate = syncTask;
        }
    }
}
