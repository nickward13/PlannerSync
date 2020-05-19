using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlannerSync.ClassLibrary;

namespace PlannerSync.XUnitTest
{
    class StubPlannerClient : IPlannerClient
    {
        public Task CompleteTaskAsync(PlannerTask plannerTask)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlannerTask>> GetTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(PlannerTask plannerTask)
        {
            throw new NotImplementedException();
        }
    }
}
