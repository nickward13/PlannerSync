using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PlannerSync.ClassLibrary;
using System.Runtime.CompilerServices;

namespace PlannerSync.XUnitTest
{
    public class SyncEngineTest
    {
        [Fact]
        public async System.Threading.Tasks.Task SyncTasksAsync_NoTasks_SyncAsync()
        {
            IPlannerClient plannerClient = new StubPlannerClient();
            IOutlookClient outlookClient = new StubOutlookClient();
            ISyncStateClient syncStateClient = new StubSyncStateClient();

            await SyncEngine.SyncTasksAsync(plannerClient, outlookClient, syncStateClient);

            int plannerTaskCount = (await plannerClient.GetTasksAsync()).Count;
            int outlookTaskCount = (await outlookClient.GetTasksAsync()).Count;
            Assert.Equal(plannerTaskCount, outlookTaskCount);
        }
        
    }
}
