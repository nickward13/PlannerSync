using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlannerSync.ClassLibrary;

namespace PlannerSync.XUnitTest
{
    class StubSyncStateClient : ISyncStateClient
    {
        public List<SyncedTask> syncedTasks = new List<SyncedTask>();

        public async Task<List<SyncedTask>> GetSavedSyncStateAsync()
        {
            return syncedTasks;
        }

        public async Task SaveSyncStateAsync(List<SyncedTask> outlookTasks)
        {
            return;
        }
    }
}
