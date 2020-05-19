using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public class SyncEngine
    {
        
        public static async Task SyncTasksAsync(IPlannerClient plannerClient, IOutlookClient outlookClient, ISyncStateClient syncStateClient)
        {
            List<PlannerTask> plannerTasks = await plannerClient.GetTasksAsync();

            List<OutlookTask> outlookTasks = await outlookClient.GetTasksAsync();

            List<SyncedTask> lastSyncedTasks = await syncStateClient.GetSavedSyncStateAsync();
            List<SyncedTask> syncedTasksToDelete = new List<SyncedTask>();
            List<SyncedTask> syncedTasksToAdd = new List<SyncedTask>();

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
                    outlookTaskToAdd = await outlookClient.AddTaskAsync(outlookTaskToAdd);
                    syncedTasksToAdd.Add(new SyncedTask() { Title = outlookTaskToAdd.Subject, OutlookId = outlookTaskToAdd.Id, PlannerId = plannerTask.Id, DueDate = plannerTask.DueDateTime});
                }
            }

            foreach(SyncedTask syncedTask in lastSyncedTasks)
            {
                if(!plannerTasks.Exists(pt => pt.Id == syncedTask.PlannerId))
                {
                    if(outlookTasks.Exists(ot => ot.Id == syncedTask.OutlookId))
                    {
                        OutlookTask outlookTaskToComplete = outlookTasks.Find(ot => ot.Id == syncedTask.OutlookId);
                        await outlookClient.CompleteOutlookTaskAsync(outlookTaskToComplete);
                    }
                    syncedTasksToDelete.Add(syncedTask);
                } else if (!outlookTasks.Exists(ot => ot.Id == syncedTask.OutlookId))
                {
                    PlannerTask plannerTaskToComplete = plannerTasks.Find(pt => pt.Id == syncedTask.PlannerId);
                    await plannerClient.CompleteTaskAsync(plannerTaskToComplete);
                    syncedTasksToDelete.Add(syncedTask);
                } else if (outlookTasks.Find(ot => ot.Id == syncedTask.OutlookId).DueDateTime.DateTime != syncedTask.DueDate)
                {
                    PlannerTask plannerTaskToUpdate = plannerTasks.Find(pt => pt.Id == syncedTask.PlannerId);
                    plannerTaskToUpdate.DueDateTime = outlookTasks.Find(ot => ot.Id == syncedTask.OutlookId).DueDateTime.DateTime;
                    await plannerClient.UpdateTaskAsync(plannerTaskToUpdate);
                    syncedTask.DueDate = outlookTasks.Find(ot => ot.Id == syncedTask.OutlookId).DueDateTime.DateTime;
                }
            }

            foreach(SyncedTask syncedTaskToDelete in syncedTasksToDelete)
            {
                lastSyncedTasks.Remove(syncedTaskToDelete);
            }

            foreach(SyncedTask syncedTaskToAdd in syncedTasksToAdd)
            {
                lastSyncedTasks.Add(syncedTaskToAdd);
            }

            await syncStateClient.SaveSyncStateAsync(lastSyncedTasks);
        }
    }
}
