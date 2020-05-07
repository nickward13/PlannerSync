using System;
using System.Collections.Generic;
using System.Linq;
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

            foreach(PlannerTask plannerTask in plannerTasks)
            {
                if(!lastSyncOutlookTasks.Exists(t => t.Subject == plannerTask.Title))
                {
                    lastSyncOutlookTasks.Add(new OutlookTask() { Subject = plannerTask.Title, DueDateTime = plannerTask.DueDateTime });
                    // insert outlook task
                } else if(!lastSyncOutlookTasks.Exists(t => t.DueDateTime == plannerTask.DueDateTime && t.Subject == plannerTask.Title))
                {
                    lastSyncOutlookTasks.Find(t => t.Subject == plannerTask.Title).DueDateTime = plannerTask.DueDateTime;
                    // update outlook task
                }
            }

            await syncStateClient.SaveSyncStateAsync(lastSyncOutlookTasks);
        }
    }
}
