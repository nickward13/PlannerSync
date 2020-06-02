using PlannerSync.ClassLibrary;
using System;
using Xunit;

namespace PlannerSync.IntegrationTests
{
    public class OutlookLogicAppClientTests
    {
        [Fact]
        public async void GetTasks_NonZeroList()
        {
            OutlookLogicAppClient outlookClient = new OutlookLogicAppClient();

            await outlookClient.RefreshTasksAsync();

            Assert.NotNull(outlookClient.Tasks);
        }

        [Fact]
        public async void AddTaskAsync_NewTask_ReturnsValidTaskId()
        {
            OutlookLogicAppClient outlookClient = new OutlookLogicAppClient();
            await outlookClient.RefreshTasksAsync();
            SyncTask syncTask = new SyncTask()
            {
                Title = "Test Task"
            };

            SyncTask syncedTask = await outlookClient.AddTaskAsync(syncTask);

            Assert.NotNull(syncedTask.Id);
        }
    }
}
