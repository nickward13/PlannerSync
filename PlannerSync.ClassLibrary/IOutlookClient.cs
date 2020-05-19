using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public interface IOutlookClient
    {
        Task<List<OutlookTask>> GetTasksAsync();
        Task<OutlookTask> AddTaskAsync(OutlookTask outlookTask);
        Task CompleteOutlookTaskAsync(OutlookTask outlookTask);
        Task UpdateOutlookTaskAsync(OutlookTask outlookTask);
    }
}
