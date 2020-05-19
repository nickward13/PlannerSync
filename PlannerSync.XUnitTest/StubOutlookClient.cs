using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlannerSync.ClassLibrary;

namespace PlannerSync.XUnitTest
{
    class StubOutlookClient : IOutlookClient
    {
        List<OutlookTask> outlookTasks = new List<OutlookTask>();

        public async Task<OutlookTask> AddTaskAsync(OutlookTask outlookTask)
        {
            outlookTask.Id = Guid.NewGuid().ToString();
            outlookTasks.Add(outlookTask);
            return outlookTask;
        }

        public Task CompleteOutlookTaskAsync(OutlookTask outlookTask)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OutlookTask>> GetTasksAsync()
        {
            return outlookTasks;
        }

        public Task UpdateOutlookTaskAsync(OutlookTask outlookTask)
        {
            throw new NotImplementedException();
        }
    }
}
