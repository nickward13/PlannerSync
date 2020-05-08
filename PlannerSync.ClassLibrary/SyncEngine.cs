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
            List<SyncedTask> syncedTasksToDelete = new List<SyncedTask>();

            foreach(PlannerTask plannerTask in plannerTasks)
            {
                if(lastSyncedTasks.Exists(syncedTask => syncedTask.PlannerId == plannerTask.Id))
                {
                    SyncedTask syncedTask = lastSyncedTasks.Find(st => st.PlannerId == plannerTask.Id);
                    if (syncedTask.DueDate != plannerTask.DueDateTime)
                    {
                        if(outlookTasks.Exists(ot => ot.Id == syncedTask.OutlookId))
                        {
                            OutlookTask outlookTaskToUpdate = outlookTasks.Find(ot => ot.Id == syncedTask.OutlookId);
                            if(plannerTask.DueDateTime == null)
                            {
                                outlookTaskToUpdate.DueDateTime = null;
                            }
                            else
                            {
                                outlookTaskToUpdate.DueDateTime = new OutlookDateTime() { DateTime = plannerTask.DueDateTime, Timezone = "UTC" };
                            }
                            await outlookClient.UpdateOutlookTaskAsync(outlookTaskToUpdate);
                            syncedTask.DueDate = plannerTask.DueDateTime;
                        }
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

            foreach(SyncedTask syncedTask in lastSyncedTasks)
            {
                if(!plannerTasks.Exists(pt => pt.Id == syncedTask.PlannerId))
                {
                    if(outlookTasks.Exists(ot => ot.Id == syncedTask.OutlookId))
                    {
                        OutlookTask outlookTaskToComplete = outlookTasks.Find(ot => ot.Id == syncedTask.OutlookId);
                        outlookTaskToComplete.Status = "Completed";
                        await outlookClient.UpdateOutlookTaskAsync(outlookTaskToComplete);
                    }
                    syncedTasksToDelete.Add(syncedTask);
                }
            }

            foreach(SyncedTask syncedTaskToDelete in syncedTasksToDelete)
            {
                lastSyncedTasks.Remove(syncedTaskToDelete);
            }

            await syncStateClient.SaveSyncStateAsync(lastSyncedTasks);
        }
    }
}
