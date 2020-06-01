using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public class SyncEngine
    {
        
        public static async Task SyncTasksAsync(ISyncTaskClient primarySyncTaskClient, ISyncTaskClient secondarySyncTaskClient, ISyncStateClient syncStateClient)
        {
            List<SyncedTask> lastSyncedTasks = await syncStateClient.GetSavedSyncStateAsync();
            List<SyncedTask> syncedTasksToDelete = new List<SyncedTask>();
            List<SyncedTask> syncedTasksToAdd = new List<SyncedTask>();

            foreach(SyncTask plannerTask in primarySyncTaskClient.Tasks)
            {
                if(lastSyncedTasks.Exists(syncedTask => syncedTask.PlannerId == plannerTask.Id))
                {
                    SyncedTask syncedTask = lastSyncedTasks.Find(st => st.PlannerId == plannerTask.Id);
                    if (syncedTask.DueDate != plannerTask.DueDateTime)
                    {
                        if(secondarySyncTaskClient.Tasks.Exists(ot => ot.Id == syncedTask.OutlookId))
                        {
                            SyncTask outlookTaskToUpdate = secondarySyncTaskClient.Tasks.Find(ot => ot.Id == syncedTask.OutlookId);
                            if(plannerTask.DueDateTime == null)
                            {
                                outlookTaskToUpdate.DueDateTime = DateTime.MinValue;
                            }
                            else
                            {
                                outlookTaskToUpdate.DueDateTime = plannerTask.DueDateTime;
                            }
                            await secondarySyncTaskClient.UpdateTaskAsync(outlookTaskToUpdate);
                            syncedTask.DueDate = plannerTask.DueDateTime;
                        }
                    }
                } else
                {
                    SyncTask outlookTaskToAdd = new SyncTask() { Title = plannerTask.Title };
                    if (plannerTask.DueDateTime != null)
                        outlookTaskToAdd.DueDateTime = plannerTask.DueDateTime;
                    outlookTaskToAdd = await secondarySyncTaskClient.AddTaskAsync(outlookTaskToAdd);
                    syncedTasksToAdd.Add(new SyncedTask() { Title = outlookTaskToAdd.Title, OutlookId = outlookTaskToAdd.Id, PlannerId = plannerTask.Id, DueDate = plannerTask.DueDateTime});
                }
            }

            foreach(var syncedTask in syncedTasksToAdd)
            {
                lastSyncedTasks.Add(syncedTask);
            }

            await syncStateClient.SaveSyncStateAsync(lastSyncedTasks);
        }
    }
}
