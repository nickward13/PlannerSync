using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlannerSync.ClassLibrary
{
    public interface IPlannerClient
    {
        Task<List<PlannerTask>> GetTasksAsync();
        Task CompleteTaskAsync(PlannerTask plannerTask);
        Task UpdateTaskAsync(PlannerTask plannerTask);
    }
}
