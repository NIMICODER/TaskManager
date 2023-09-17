using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Entities.Enums;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.Tasks.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Tasks
{
    public interface ITaskService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<ServiceResponse<TaskDto>> GetTaskAsync(Guid taskId);
    
       /// <summary>
       /// creating task
       /// </summary>
       /// <param name="projectId"></param>
       /// <param name="taskDto"></param>
       /// <returns></returns>
        Task<ServiceResponse<TaskDto>> CreateTaskAsync(Guid projectId, TaskCreateDto request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //Task<ServiceResponse<TaskDto>> CreateTaskAsync(TaskCreateDto request);
        /// <summary>
        /// Updating task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="projectId"></param>
        /// <param name="taskDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<TaskDto>> UpdateTaskAsync(Guid taskId, Guid projectId, TaskUpdateDto request);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="taskId"></param>
       /// <param name="projectId"></param>
       /// <returns></returns>
        Task<ServiceResponse<TaskDto>> DeleteTaskAsync(Guid taskId, Guid projectId);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="projectId"></param>
       /// <param name="requestParameters"></param>
       /// <param name="status"></param>
       /// <returns></returns>
        Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByStatusAsync(Guid projectId, RequestParameters requestParameters, TasksStatus status);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByPriorityAsync(TaskPriority priority, Guid projectId, RequestParameters requestParameters);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksDueThisWeekAsync(Guid projectId, RequestParameters requestParameters);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="taskId"></param>
       /// <param name="projectId"></param>
       /// <returns></returns>
        Task<ServiceResponse<TaskDto>> RemoveTaskFromProjectAsync(Guid taskId, Guid projectId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<ServiceResponse<TaskDto>> AssignTaskToProjectAsync(Guid taskId, Guid projectId);
        Task<ServiceResponse<TaskDto>> ToggleTaskStatusAsync(Guid taskId, string userId, TasksStatus status);
    }
}
