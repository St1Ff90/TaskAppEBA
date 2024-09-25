using BL.Models.DTO;
using BL.Models.Filters;
using BL.Models.Requests;
using BL.Services.TaskService;
using DAL.Entities;
using DAL.Repositories.TaskRepository.TaskRepository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.Tests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepositoryMock;
        private Mock<ILogger<TaskService>> _loggerMock;
        private TaskService _taskService;

        [SetUp]
        public void SetUp()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _loggerMock = new Mock<ILogger<TaskService>>();
            _taskService = new TaskService(_taskRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetUserTasksAsync_WhenCalled_ShouldReturnTasks()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var filter = new TaskFilter { PageNumber = 1, PageSize = 10, SortBy = Core.Models.SortField.Priority, isAsc = true };
            var tasks = new List<UserTask>
            {
                new UserTask {Id = Guid.NewGuid(), UserId = userId, Title = "Test Task 1", Status = (int)Status.Pending, Priority = (int)Priority.Low},
                new UserTask { Id = Guid.NewGuid(), UserId = userId, Title = "Test Task 2", Status = (int)Status.Pending, Priority = (int)Priority.Low }
            };

            _taskRepositoryMock.Setup(repo => repo.GetUserTasksAsunc(It.IsAny<Expression<Func<UserTask, bool>>>(), filter.PageNumber, filter.PageSize, Core.Models.SortField.Priority, true))
                               .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetUserTasksAsync(userId, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task CreateUserTaskAsync_WhenCalled_ShouldCreateTask()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UserTaskRequest { Title = "New Task", Description = "Task Description", Status = Status.Pending, Priority = Priority.Low };
            var newTask = new UserTask { Id = Guid.NewGuid(), UserId = userId, Title = "New Task", Status = (int)Status.Pending, Priority = (int)Priority.Low };

            // Set up the TaskMapper mapping
            _taskRepositoryMock.Setup(m => m.CreateAsync(It.IsAny<UserTask>()))
                               .ReturnsAsync(newTask);

            // Act
            var result = await _taskService.CreateUserTaskAsync(userId, request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(request.Title, result.Title);
            _taskRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<UserTask>(task => task.Title == request.Title && task.UserId == userId)), Times.Once);
        }

        [Test]
        public async Task DeleteUserTaskAsync_WhenTaskExists_ShouldReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.DeleteUserTaskAsync(userId, taskId))
                               .ReturnsAsync(true);

            // Act
            var result = await _taskService.DeleteUserTaskAsync(userId, taskId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetUserTaskByIdAsync_WhenTaskExists_ShouldReturnTask()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var task = new UserTask { Id = taskId, UserId = userId, Title = "Task", Status = (int)Status.Pending, Priority = (int)Priority.Low };

            _taskRepositoryMock.Setup(repo => repo.GetUserTaskAsync(userId, taskId))
                               .ReturnsAsync(task);

            // Act
            var result = await _taskService.GetUserTaskByIdAsync(userId, taskId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(task.Title, result.Title);
        }

        [Test]
        public async Task UpdateUserTaskAsync_WhenTaskExists_ShouldUpdateTask()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var request = new UserTaskRequest { Title = "Updated Task", Description = "Updated Description", Status = Status.Pending, Priority = Priority.Low };
            var existingTask = new UserTask { Id = taskId, UserId = userId, Title = "Old Task", Status = (int)Status.Pending, Priority = (int)Priority.Low };

            _taskRepositoryMock.Setup(repo => repo.GetUserTaskAsync(userId, taskId))
                               .ReturnsAsync(existingTask);
            _taskRepositoryMock.Setup(repo => repo.UpdateUserTaskAsync(It.IsAny<UserTask>()))
                               .ReturnsAsync(existingTask);

            // Act
            var result = await _taskService.UpdateUserTaskAsync(userId, taskId, request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(request.Title, result.Title);
        }

        [Test]
        public async Task GetUserTaskByIdAsync_WhenTaskDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.GetUserTaskAsync(userId, taskId))
                               .ReturnsAsync((UserTask)null);

            // Act
            var result = await _taskService.GetUserTaskByIdAsync(userId, taskId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteUserTaskAsync_WhenTaskDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.DeleteUserTaskAsync(userId, taskId))
                               .ReturnsAsync(false);

            // Act
            var result = await _taskService.DeleteUserTaskAsync(userId, taskId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
