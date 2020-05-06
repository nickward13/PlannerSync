using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public class SyncEngine
    {
        public static async Task SyncTasksAsync()
        {
            PlannerClient plannerClient = new PlannerClient();
            List<PlannerTask> plannerTasks = await plannerClient.GetTasksAsync();

            OutlookClient outlookClient = new OutlookClient();
            List<OutlookTask> outlookTasks = await outlookClient.GetTasksAsync();

            ISyncStateClient syncStateClient = new BlobSyncStateClient();
            List<OutlookTask> lastSyncOutlookTasks = await syncStateClient.GetSavedSyncStateAsync();

            await syncStateClient.SaveSyncStateAsync(lastSyncOutlookTasks);
        }
    }
}
