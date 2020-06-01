using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PlannerSync.ClassLibrary;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PlannerSync.XUnitTest
{
    public class SyncEngineTest
    {
        ISyncTaskClient _plannerClient = new StubSyncTaskClient();
        ISyncTaskClient _outlookClient = new StubSyncTaskClient();
        StubSyncStateClient _syncStateClient = new StubSyncStateClient();

        [Fact]
        public async Task SyncTasksAsync_NoTasks_EqualTaskCount()
        {
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal(_plannerClient.Tasks.Count, _outlookClient.Tasks.Count);
        }

        /*
        StubPlannerClient plannerClient = new StubPlannerClient();
        StubOutlookClient outlookClient = new StubOutlookClient();
        StubSyncStateClient syncStateClient = new StubSyncStateClient();

        [Fact]
        public async System.Threading.Tasks.Task SyncTasksAsync_NoTasks_EqualTaskCount()
        {
            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_OnePlannerTask_EqualTaskCount()
        {
            AddPlannerTask("A Task");

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPlannerTask_EqualTaskCount()
        {
            AddPlannerTask("A Task");
            AddPlannerTask("Another task");

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPlannerTasksOverTwoSyncs_EqualTasks()
        {
            AddPlannerTask("A task");
            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);
            AddPlannerTask("Another Task");

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_ThreePlannerTasksOverThreeSyncs_EqualTasks()
        {
            AddPlannerTask("Task 1");
            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);
            AddPlannerTask("Task 2");
            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);
            AddPlannerTask("Task 3");

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_UpdatePlannerTaskDueDate_DatesEqual()
        {
            AddPlannerTask("Task 1");
            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);
            var taskToUpdate = plannerClient.GetTasksAsync().Result[0];
            taskToUpdate.DueDateTime = DateTime.Now.ToString();
            await plannerClient.UpdateTaskAsync(taskToUpdate);

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            string plannerTaskDueDate = plannerClient.GetTasksAsync().Result[0].DueDateTime;
            string outlookTaskDueDate = outlookClient.GetTasksAsync().Result[0].DueDateTime.DateTime;
            Assert.Equal(plannerTaskDueDate, outlookTaskDueDate);
        }

        private void AddPlannerTask(string title)
        {
            plannerClient.AddTask(
                            new PlannerTask()
                            {
                                PercentComplete = 0,
                                Title = title
                            });
        }
        */
    }
}
