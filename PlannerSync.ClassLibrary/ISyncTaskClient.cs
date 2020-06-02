using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public interface ITaskSyncable
    {
        List<SyncTask> Tasks { get; set; }
        
        Task CompleteTaskAsync(SyncTask syncTask);
        Task UpdateTaskAsync(SyncTask syncTask);
        Task<SyncTask> AddTaskAsync(SyncTask syncTask);
    }
}
