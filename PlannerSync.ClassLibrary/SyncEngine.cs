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

            foreach(SyncTask task in primarySyncTaskClient.Tasks)
            {
                if(!lastSyncedTasks.Exists(t => t.PrimaryTaskId == task.Id))
                {
                    SyncTask newTask = await secondarySyncTaskClient.AddTaskAsync(task);
                    syncedTasksToAdd.Add(new SyncedTask() { Title = newTask.Title, SecondaryTaskId = newTask.Id, PrimaryTaskId = task.Id, DueDateTime = task.DueDateTime });
                } else
                {
                    SyncedTask lastSyncedTask = lastSyncedTasks.Find(st => st.PrimaryTaskId == task.Id);
                    if (!IsTaskContentEqual(task, lastSyncedTask))
                    {
                        lastSyncedTask.DueDateTime = task.DueDateTime;
                        lastSyncedTask.Description = task.Description;
                        lastSyncedTask.Title = task.Title;
                        if (secondarySyncTaskClient.Tasks.Exists(ot => ot.Id == lastSyncedTask.SecondaryTaskId))
                        {
                            SyncTask secondaryTaskToUpdate = secondarySyncTaskClient.Tasks.Find(ot => ot.Id == lastSyncedTask.SecondaryTaskId);
                            secondaryTaskToUpdate.DueDateTime = task.DueDateTime;
                            secondaryTaskToUpdate.Description = task.Description;
                            secondaryTaskToUpdate.Title = task.Title;
                            await secondarySyncTaskClient.UpdateTaskAsync(secondaryTaskToUpdate);
                        }
                    }
                }
            }

            foreach(var syncedTask in lastSyncedTasks)
            {
                if (!primarySyncTaskClient.Tasks.Exists(t => t.Id == syncedTask.PrimaryTaskId))
                    syncedTasksToDelete.Add(syncedTask);
            }

            foreach(var syncedTask in syncedTasksToDelete)
            {
                await secondarySyncTaskClient.CompleteTaskAsync(secondarySyncTaskClient.Tasks.First(t => t.Id == syncedTask.SecondaryTaskId));
                lastSyncedTasks.Remove(syncedTask);
            }

            foreach(var syncedTask in syncedTasksToAdd)
            {
                lastSyncedTasks.Add(syncedTask);
            }

            await syncStateClient.SaveSyncStateAsync(lastSyncedTasks);
        }

        private static bool IsTaskContentEqual(SyncTask referenceTask, SyncedTask taskToCompare)
        {
            if (referenceTask.Description == taskToCompare.Description
                && referenceTask.DueDateTime == taskToCompare.DueDateTime
                && referenceTask.Title == taskToCompare.Title)
                return true;
            else
                return false;
        }
    }
}
