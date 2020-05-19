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
        StubPlannerClient plannerClient = new StubPlannerClient();
        StubOutlookClient outlookClient = new StubOutlookClient();
        StubSyncStateClient syncStateClient = new StubSyncStateClient();

        [Fact]
        public async System.Threading.Tasks.Task SyncTasksAsync_NoTasks_SyncAsync()
        {
            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_OnePlannerTask_SyncAsync()
        {
            plannerClient.AddTask(
                new PlannerTask() {  
                    PercentComplete = 0, Title = "A Task" 
                });

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPlannerTask_SyncAsync()
        {
            plannerClient.AddTask(
                new PlannerTask()
                {
                    PercentComplete = 0,
                    Title = "A Task"
                });
            plannerClient.AddTask(
                new PlannerTask()
                {
                    PercentComplete = 0,
                    Title = "Another Task"
                });

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPlannerTask_OverTwoSyncAsync()
        {
            plannerClient.AddTask(
                new PlannerTask()
                {
                    PercentComplete = 0,
                    Title = "A Task"
                });
            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);
            plannerClient.AddTask(
                new PlannerTask()
                {
                    PercentComplete = 0,
                    Title = "Another Task"
                });

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }
    }
}
