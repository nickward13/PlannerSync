using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlannerSync.ClassLibrary;

namespace PlannerSync.XUnitTest
{
    class StubOutlookClient : IOutlookClient
    {
        public Task<OutlookTask> AddTaskAsync(OutlookTask outlookTask)
        {
            throw new NotImplementedException();
        }

        public Task CompleteOutlookTaskAsync(OutlookTask outlookTask)
        {
            throw new NotImplementedException();
        }

        public Task<List<OutlookTask>> GetTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateOutlookTaskAsync(OutlookTask outlookTask)
        {
            throw new NotImplementedException();
        }
    }
}
