using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PlannerSync.ClassLibrary;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq;

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

        [Fact]
        public async Task SyncTasksAsync_OnePlannerTask_EqualTaskCount()
        {
            await AddPlannerTaskAsync("A Task");

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Single(_outlookClient.Tasks);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPlannerTask_EqualTaskCount()
        {
            await AddPlannerTaskAsync("A Task");
            await AddPlannerTaskAsync("Another task");

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal(2, _outlookClient.Tasks.Count);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPlannerTasksOverTwoSyncs_EqualTasks()
        {
            await AddPlannerTaskAsync("A task");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            await AddPlannerTaskAsync("Another Task");

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal(2, _outlookClient.Tasks.Count);
        }

        [Fact]
        public async Task SyncTasksAsync_ThreePlannerTasksOverThreeSyncs_EqualTasks()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            await AddPlannerTaskAsync("Task 2");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            await AddPlannerTaskAsync("Task 3");

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal(3, _outlookClient.Tasks.Count);
        }

        [Fact]
        public async Task SyncTasksAsync_AddThenRemovePrimaryTask_EqualTasks()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            var taskToComplete = _plannerClient.Tasks.First();
            await _plannerClient.CompleteTaskAsync(taskToComplete);

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Empty(_outlookClient.Tasks);
        }

        [Fact]
        public async Task SyncTasksAsync_AddTwoThenCompletePrimaryTask_TasksEqual1()
        {
            await AddPlannerTaskAsync("Task 1");
            await AddPlannerTaskAsync("Task 2");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            await _plannerClient.CompleteTaskAsync(_plannerClient.Tasks[1]);

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Single(_outlookClient.Tasks);
        }

        [Fact]
        public async Task SyncTaskAsync_SyncTask_TaskIdsNotEqual()
        {
            await AddPlannerTaskAsync("Task 1");

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.NotEqual(_plannerClient.Tasks[0].Id, _outlookClient.Tasks[0].Id);
        }

        [Fact]
        public async Task SyncTaskAsync_SyncTask_TaskTitlesAreEqual()
        {
            await AddPlannerTaskAsync("Task 1");

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal(_plannerClient.Tasks[0].Title, _outlookClient.Tasks[0].Title);
        }

        [Fact]
        public async Task SyncTasksAsync_UpdatePlannerTaskDueDate_DatesEqual()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            _plannerClient.Tasks[0].DueDateTime = DateTime.Now;

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal(_plannerClient.Tasks[0].DueDateTime, _outlookClient.Tasks[0].DueDateTime);
        }

        [Fact]
        public async Task SyncTasksAsync_UpdatePrimaryTitle_TitlesEqual()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            _plannerClient.Tasks[0].Title = "Updated Title";

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal("Updated Title", _outlookClient.Tasks[0].Title);
        }

        [Fact]
        public async Task SyncTasksAsync_UpdatePrimaryDescription_DatesEqual()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);
            _plannerClient.Tasks[0].Description = "A new description";

            await SyncEngine.SyncTasksAsync(_plannerClient, _outlookClient, _syncStateClient);

            Assert.Equal("A new description", _outlookClient.Tasks[0].Description);
        }
        
        private async Task AddPlannerTaskAsync(string title)
        {
            await _plannerClient.AddTaskAsync(
                            new SyncTask()
                            {
                                Title = title
                            });
        }
        
    }
}
