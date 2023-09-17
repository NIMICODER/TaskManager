using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Data.Interfaces;
using TaskManager_Models.Entities.Domains.Projects;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Models.Entities.Enums;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.ServiceFactory;
using TaskManager_Services.Domains.Tasks.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TaskTodo> _taskRepo;
        private readonly IRepository<Project> _projectRepo;
        private readonly IRepository<ApplicationUser> _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public TaskService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _taskRepo = _unitOfWork.GetRepository<TaskTodo>();
            _projectRepo = _unitOfWork.GetRepository<Project>();
            _userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        }




        //public async Task<ServiceResponse<TaskDto>> CreateTaskAsync(TaskCreateDto request)
        //{
        //    bool taskExist = await _taskRepo.AnyAsync(t => t.Description.ToLower() == request.Description.ToLower());
        //    if (taskExist)
        //    {
        //        return new ServiceResponse<TaskDto>
        //        {
        //            Message = "Task already exist",
        //            StatusCode = HttpStatusCode.BadRequest
        //        };
        //    }
        //    var newTask = new TaskTodo
        //    {
        //        Title = request.Title,
        //        Description = request.Description,
        //        DueDate = request.DueDate,
        //        Priority = request.Priority,
        //        Status = request.Status
        //    };
        //    newTask.Status = TasksStatus.Completed;
        //    _taskRepo.Add(newTask);
        //    int rows = _unitOfWork.SaveChanges();
        //    if (rows > 0)
        //    {
        //        // Mapping the new task to a TaskDto
        //        var taskDto = new TaskDto
        //        (
        //            newTask.Id, // Assign the new task's ID
        //            newTask.Title,
        //            newTask.Description,
        //            newTask.DueDate,
        //            newTask.Priority,
        //            newTask.Status
        //        );

        //        return new ServiceResponse<TaskDto>
        //        {
        //            Data = taskDto,
        //            Message = "Task created successfully",
        //            StatusCode = HttpStatusCode.Created // Changed the status code to Created
        //        };
        //    }
        //    else
        //    {
        //        return new ServiceResponse<TaskDto>
        //        {
        //            Message = "Something went wrong while creating the task",
        //            StatusCode = HttpStatusCode.InternalServerError // Changed the status code to InternalServerError
        //        };
        //    }

        //}

        public async Task<ServiceResponse<TaskDto>> CreateTaskAsync(Guid projectId, TaskCreateDto request)
        {
           Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);
            if (project == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var newTask = new TaskTodo
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
                ProjectId = project.Id,
                Status = request.Status,
            };

            newTask.ProjectId = project.Id;
            _taskRepo.Add( newTask );   
            int rows = await _unitOfWork.SaveChangesAsync();
            
            if(rows > 0)
            {
                var createdTaskDto = new TaskDto
                (
                   newTask.Id,
                   newTask.Title,
                   newTask.Description,
                   newTask.DueDate,
                   newTask.Priority,
                   newTask.Status
                );

                return new ServiceResponse<TaskDto>
                {
                    Data = createdTaskDto,
                    Message = "Task created successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Something went wrong while creating the task",
                    StatusCode = HttpStatusCode.InternalServerError
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

        public async Task<ServiceResponse<TaskDto>> DeleteTaskAsync(Guid taskId, Guid projectId)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);

            if (project == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            TaskTodo taskToDelete = await _taskRepo.GetSingleByAsync(t => t.Id == taskId && t.ProjectId == projectId);

            if (taskToDelete == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Task not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            // Delete the task from the repository
            await _taskRepo.DeleteAsync(taskToDelete);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Task deleted successfully",
                    StatusCode = HttpStatusCode.NoContent
                };
            }
            else
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Something went wrong while deleting the task",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

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

        public Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByPriorityAsync(TaskPriority priority, Guid projectId, RequestParameters requestParameters)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByStatusAsync(Guid projectId, RequestParameters requestParameters, TasksStatus status)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);

            if (project == null)
            {
                return new ServiceResponse<PaginationResponse<TaskDto>>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            PaginationResult<TaskTodo> tasks = await _taskRepo.GetPagedItems(
                requestParameters,
                predicate: x => x.ProjectId == projectId && x.Status == status,
                orderBy: o => o.OrderBy(x => x.Title)
            );

            var taskDtos = tasks.Records.Select(task => new TaskDto
            (
               task.Id,
               task.Title,
               task.Description,
               task.DueDate,
               task.Priority,
               task.Status
                )
            );

            var paginationResponse = new PaginationResponse<TaskDto>(
                tasks.PageSize,
                tasks.CurrentPage,
                tasks.TotalPages,
                tasks.TotalRecords,
                taskDtos
            );

            return new ServiceResponse<PaginationResponse<TaskDto>>
            {
                Data = paginationResponse,
                Message = $"{tasks.TotalRecords} tasks found with status {status}",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksDueThisWeekAsync(Guid projectId, RequestParameters requestParameters)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);

            if (project == null)
            {
                return new ServiceResponse<PaginationResponse<TaskDto>>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            DateTime today = DateTime.Now;
            DateTime endOfWeek = today.AddDays(7);

            PaginationResult<TaskTodo> tasks = await _taskRepo.GetPagedItems(
                requestParameters,
                predicate: x => x.ProjectId == projectId && x.DueDate >= today && x.DueDate <= endOfWeek,
                orderBy: o => o.OrderBy(x => x.Title)
            );

            var taskDtos = tasks.Records.Select(task => new TaskDto
            (
                task.Id,
                task.Title,
                task.Description,
                task.DueDate,
                task.Priority,
                task.Status
                )
            );

            var paginationResponse = new PaginationResponse<TaskDto>(
                tasks.PageSize,
                tasks.CurrentPage,
                tasks.TotalPages,
                tasks.TotalRecords,
                taskDtos
            );

            return new ServiceResponse<PaginationResponse<TaskDto>>
            {
                Data = paginationResponse,
                Message = $"{tasks.TotalRecords} tasks due this week",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<TaskDto>> ToggleTaskStatusAsync(Guid taskId, string userId, TasksStatus status)
        {
            TaskTodo task = await _taskRepo.GetSingleByAsync(t => t.Id == taskId);

            if (task == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Task not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "User not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            task.Status = status;

            await _taskRepo.UpdateAsync(task);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = new TaskDto
                    (
                        task.Id,
                        task.Title,
                        task.Description,
                        task.DueDate,
                        task.Priority,
                        task.Status
                    ),
                    Message = "Task status updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Something went wrong while updating the task status",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ServiceResponse<TaskDto>> UpdateTaskAsync(Guid taskId, Guid projectId, TaskUpdateDto request)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);
            if (project == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            TaskTodo existingTask = await _taskRepo.GetSingleByAsync(t => t.Id == taskId);

            if (existingTask == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Task not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            existingTask.Title = request.Title;
            existingTask.Description = request.Description;
            existingTask.DueDate = request.DueDate;
            existingTask.Priority = request.Priority;

            await _taskRepo.UpdateAsync(existingTask);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
            {
                // Map the updated task to TaskDto
                var updatedTaskDto = new TaskDto
                (
                    existingTask.Id,
                    existingTask.Title,
                    existingTask.Description,
                    existingTask.DueDate,
                    existingTask.Priority,
                    existingTask.Status
                );

                return new ServiceResponse<TaskDto>
                {
                    Data = updatedTaskDto,
                    Message = "Task updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Something went wrong while updating the task",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

        }

        public async Task<ServiceResponse<TaskDto>> AssignTaskToProjectAsync(Guid taskId, Guid projectId)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);
            if (project == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Project not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            TaskTodo task = await _taskRepo.GetSingleByAsync(t => t.Id == taskId);

            if (task == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Task not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            task.ProjectId = projectId;

            await _taskRepo.UpdateAsync(task);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
            {
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
                    Message = $"Task assigned to {project.Name} successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Something went wrong",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ServiceResponse<TaskDto>> RemoveTaskFromProjectAsync(Guid taskId, Guid projectId)
        {
            // Check if the task exists and if it belongs to the specified project
            TaskTodo task = await _taskRepo.GetSingleByAsync(t => t.Id == taskId && t.ProjectId == projectId);

            if (task == null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Task not found or does not belong to this project",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            task.ProjectId = null;

            await _taskRepo.UpdateAsync(task);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
            {
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
                    Message = $"{task.Project.Name} removed from project successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponse<TaskDto>
                {
                    Data = null,
                    Message = "Something went wrong ",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
