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

            OutlookTask outlookTaskToAdd;
            foreach(PlannerTask plannerTask in plannerTasks)
            {
                if(lastSyncOutlookTasks.Exists(
                    t => t.Subject == plannerTask.Title && plannerTask.DueDateTime == null && t.DueDateTime == null
                    ||
                    t.Subject == plannerTask.Title && plannerTask.DueDateTime == t.DueDateTime.DateTime))
                { 
                } else
                {
                    if (plannerTask.DueDateTime == null)
                    {
                        outlookTaskToAdd = new OutlookTask() { Subject = plannerTask.Title };
                    }
                    else
                    {
                        outlookTaskToAdd = new OutlookTask() { Subject = plannerTask.Title, DueDateTime = new OutlookDateTime() { DateTime = plannerTask.DueDateTime, Timezone = "UTC" } };
                    }
                    lastSyncOutlookTasks.Add(outlookTaskToAdd);
                    await outlookClient.AddOutlookTaskAsync(outlookTaskToAdd);
                    
                }
            }

            await syncStateClient.SaveSyncStateAsync(lastSyncOutlookTasks);
        }
    }
}
