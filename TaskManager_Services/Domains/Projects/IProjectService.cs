using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.Projects.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Projects
{
    public interface IProjectService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<ServiceResponse<ProjectDto>> GetProjectAsync(Guid projectId, string userId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<PaginationResponse<ProjectDto>>> GetAllProjectsAsync(RequestParameters requestParameters );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<ProjectDto>> CreateProjectAsync(string userId, ProjectCreateDto request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        Task<ServiceResponse<ProjectDto>> UpdateProjectAsync(Guid projectId,string userId, ProjectUpdateDto projectDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<ServiceResponse<ProjectDto>> DeleteProjectAsync(Guid projectId, string userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<ProjectDto>>> GetProjectsByUserAsync(Guid userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<bool> AddTaskToProjectAsync(Guid projectId, Guid taskId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<bool> RemoveTaskFromProjectAsync(Guid projectId, Guid taskId);

       
    }
}
