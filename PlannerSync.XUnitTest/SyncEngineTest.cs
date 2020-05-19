using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PlannerSync.ClassLibrary;

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


        }
        
    }
}
