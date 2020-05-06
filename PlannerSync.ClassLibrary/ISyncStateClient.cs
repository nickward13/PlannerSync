using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    internal interface ISyncStateClient
    {
        Task<List<OutlookTask>> GetSavedSyncStateAsync();
        Task SaveSyncStateAsync(List<OutlookTask> outlookTasks);
    }
}
