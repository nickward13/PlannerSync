using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlannerSync.ClassLibrary;

namespace PlannerSync.XUnitTest
{
    class StubPlannerClient : IPlannerClient
    {
        List<PlannerTask> plannerTasks = new List<PlannerTask>();

        public void AddTask(PlannerTask plannerTask)
        {
            plannerTask.Id = Guid.NewGuid().ToString();
            plannerTask.CreatedDateTime = DateTime.Now.ToString();
            plannerTasks.Add(plannerTask);
        }

        public Task CompleteTaskAsync(PlannerTask plannerTask)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PlannerTask>> GetTasksAsync()
        {
            return plannerTasks;
        }

        public async Task UpdateTaskAsync(PlannerTask plannerTask)
        {
            var taskToUpdate = plannerTasks.First(t => t.Id == plannerTask.Id);
            taskToUpdate = plannerTask;
        }
    }
}
