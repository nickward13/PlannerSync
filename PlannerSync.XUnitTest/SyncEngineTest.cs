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
        ISyncTaskClient _primaryClient = new StubSyncTaskClient();
        ISyncTaskClient _secondaryClient = new StubSyncTaskClient();
        StubSyncStateClient _syncStateClient = new StubSyncStateClient();

        [Fact]
        public async Task SyncTasksAsync_NoTasks_EqualTaskCount()
        {
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal(_primaryClient.Tasks.Count, _secondaryClient.Tasks.Count);
        }

        [Fact]
        public async Task SyncTasksAsync_OnePrimaryTask_EqualTaskCount()
        {
            await AddPlannerTaskAsync("A Task");

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Single(_secondaryClient.Tasks);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPrimaryTask_EqualTaskCount()
        {
            await AddPlannerTaskAsync("A Task");
            await AddPlannerTaskAsync("Another task");

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal(2, _secondaryClient.Tasks.Count);
        }

        [Fact]
        public async Task SyncTasksAsync_TwoPrimaryTasksOverTwoSyncs_EqualTasks()
        {
            await AddPlannerTaskAsync("A task");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            await AddPlannerTaskAsync("Another Task");

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal(2, _secondaryClient.Tasks.Count);
        }

        [Fact]
        public async Task SyncTasksAsync_ThreePrimaryTasksOverThreeSyncs_EqualTasks()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            await AddPlannerTaskAsync("Task 2");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            await AddPlannerTaskAsync("Task 3");

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal(3, _secondaryClient.Tasks.Count);
        }

        [Fact]
        public async Task SyncTasksAsync_AddThenCompletePrimaryTask_EqualTasks()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            var taskToComplete = _primaryClient.Tasks.First();
            await _primaryClient.CompleteTaskAsync(taskToComplete);

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Empty(_secondaryClient.Tasks);
        }

        [Fact]
        public async Task SyncTasksAsync_AddTwoThenCompletePrimaryTask_TasksEqual1()
        {
            await AddPlannerTaskAsync("Task 1");
            await AddPlannerTaskAsync("Task 2");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            await _primaryClient.CompleteTaskAsync(_primaryClient.Tasks[1]);

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Single(_secondaryClient.Tasks);
        }

        [Fact]
        public async Task SyncTaskAsync_SyncTask_TaskIdsNotEqual()
        {
            await AddPlannerTaskAsync("Task 1");

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.NotEqual(_primaryClient.Tasks[0].Id, _secondaryClient.Tasks[0].Id);
        }

        [Fact]
        public async Task SyncTaskAsync_SyncTask_TaskTitlesAreEqual()
        {
            await AddPlannerTaskAsync("Task 1");

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal(_primaryClient.Tasks[0].Title, _secondaryClient.Tasks[0].Title);
        }

        [Fact]
        public async Task SyncTasksAsync_UpdatePrimaryDueDate_DatesEqual()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            _primaryClient.Tasks[0].DueDateTime = DateTime.Now;

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal(_primaryClient.Tasks[0].DueDateTime, _secondaryClient.Tasks[0].DueDateTime);
        }

        [Fact]
        public async Task SyncTasksAsync_UpdatePrimaryTitle_TitlesEqual()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            _primaryClient.Tasks[0].Title = "Updated Title";

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal("Updated Title", _secondaryClient.Tasks[0].Title);
        }

        [Fact]
        public async Task SyncTasksAsync_UpdatePrimaryDescription_DatesEqual()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            _primaryClient.Tasks[0].Description = "A new description";

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Equal("A new description", _secondaryClient.Tasks[0].Description);
        }
        
        [Fact]
        public async Task SyncTasksAsync_CompleteTaskInSecondary_TaskGoneInPrimary()
        {
            await AddPlannerTaskAsync("Task 1");
            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);
            await _secondaryClient.CompleteTaskAsync(_secondaryClient.Tasks[0]);

            await SyncEngine.SyncTasksAsync(_primaryClient, _secondaryClient, _syncStateClient);

            Assert.Empty(_primaryClient.Tasks);
        }

        private async Task AddPlannerTaskAsync(string title)
        {
            await _primaryClient.AddTaskAsync(
                            new SyncTask()
                            {
                                Title = title
                            });
        }
        
    }
}
