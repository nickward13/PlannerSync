using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public interface ISyncStateClient
    {
        Task<List<SyncedTask>> GetSavedSyncStateAsync();
        Task SaveSyncStateAsync(List<SyncedTask> outlookTasks);
    }
}
