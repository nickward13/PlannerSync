using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlannerSync.ClassLibrary;

namespace PlannerSync.XUnitTest
{
    class StubSyncStateClient : ISyncStateClient
    {
        public Task<List<SyncedTask>> GetSavedSyncStateAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveSyncStateAsync(List<SyncedTask> outlookTasks)
        {
            throw new NotImplementedException();
        }
    }
}
