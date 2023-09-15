using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Data.Interfaces;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Models.Entities.Enums;
using TaskManager_Services.Domains.ServiceFactory;
using TaskManager_Services.Domains.Tasks.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;
        private readonly IRepository<TaskTodo> _taskRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public TaskService(IServiceFactory serviceFactory, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
            _mapper = _serviceFactory.GetService<IMapper>();
            _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
            _taskRepo = _unitOfWork.GetRepository<TaskTodo>();
        }

        public Task<bool?> AssignTaskToProjectAsync(Guid taskId, Guid projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<TaskDto>> CreateTaskAsync(TaskCreateDto request)
        {
            bool taskExist = await _taskRepo.AnyAsync(t => t.Description.ToLower() == request.Description.ToLower());
            if (taskExist)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Task already exist",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var newTask = new TaskTodo
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
            };
            newTask.Status = TaskStatus.Running;
            _taskRepo.Add(newTask);
            int rows = _unitOfWork.SaveChanges();
            if (rows > 0)
            {
                // Mapping the new task to a TaskDto
                var taskDto = new TaskDto
                (
                    newTask.Id, // Assign the new task's ID
                    newTask.Title,
                    newTask.Description,
                    newTask.DueDate,
                    newTask.Priority,
                    newTask.Status
                );

                return new ServiceResponse<TaskDto>
                {
                    Data = taskDto,
                    Message = "Task created successfully",
                    StatusCode = HttpStatusCode.Created // Changed the status code to Created
                };
            }
            else
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Something went wrong when creating the task",
                    StatusCode = HttpStatusCode.InternalServerError // Changed the status code to InternalServerError
                };
            }

        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
           TaskTodo task = await _taskRepo.GetSingleByAsync(t => t.Id == taskId);
            if (task == null)
            {
                return;
            }
            await _taskRepo.DeleteAsync(task);
        }

       

        public async Task<ServiceResponse<TaskDto>> GetTaskAsync(Guid taskId)
        {
            TaskTodo task = await _taskRepo.GetSingleByAsync(x => x.Id == taskId);
            if (task == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Task not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            var taskDto = new TaskDto
            (
                task.Id,
                 task.Title,
                task.Description,
                task.DueDate,
                task.Priority,
                task.Status
            );
            return new ServiceResponse<TaskDto>
            {
                Data = taskDto,
                Message = "Task retrieved successfully",
                StatusCode = HttpStatusCode.OK
            };


        }

        public Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksByPriorityAsync(TaskPriority priority)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksByStatusAsync(TaskStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksDueThisWeekAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool?> RemoveTaskFromProjectAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<TaskDto>> UpdateTaskAsync(Guid taskId, TaskUpdateDto taskDto)
        {
            throw new NotImplementedException();
        }
    }
}
