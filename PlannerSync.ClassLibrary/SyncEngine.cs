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
            List<SyncedTask> lastSyncedTasks = await syncStateClient.GetSavedSyncStateAsync();

            foreach(PlannerTask plannerTask in plannerTasks)
            {
                if(lastSyncedTasks.Exists(syncedTask => syncedTask.PlannerId == plannerTask.Id))
                { 
                    if(lastSyncedTasks.Find(syncedTask => syncedTask.PlannerId == plannerTask.Id).DueDate != plannerTask.DueDateTime)
                    {
                        // update outlook and synced task
                    }
                } else
                {
                    OutlookTask outlookTaskToAdd = new OutlookTask() { Subject = plannerTask.Title };
                    if(plannerTask.DueDateTime != null)
                        outlookTaskToAdd.DueDateTime = new OutlookDateTime() { DateTime = plannerTask.DueDateTime, Timezone = "UTC" };
                    outlookTaskToAdd = await outlookClient.AddOutlookTaskAsync(outlookTaskToAdd);
                    lastSyncedTasks.Add(new SyncedTask() { Title = outlookTaskToAdd.Subject, OutlookId = outlookTaskToAdd.Id, PlannerId = plannerTask.Id, DueDate = plannerTask.DueDateTime});
                }
            }

            await syncStateClient.SaveSyncStateAsync(lastSyncedTasks);
        }
    }
}
