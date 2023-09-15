using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Entities.Enums;
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
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<TaskDto>> CreateTaskAsync(TaskCreateDto taskDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<TaskDto>> UpdateTaskAsync(Guid taskId, TaskUpdateDto taskDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task DeleteTaskAsync(Guid taskId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksByStatusAsync(TaskStatus status);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksByPriorityAsync(TaskPriority priority);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<TaskDto>>> GetTasksDueThisWeekAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<bool?> AssignTaskToProjectAsync(Guid taskId, Guid projectId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<bool?> RemoveTaskFromProjectAsync(Guid taskId);
    }
}
