using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Data.Interfaces;
using TaskManager_Models.Entities.Domains.Projects;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskManager_Models.Entities.Domains.User;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.Projects.Dtos;
using TaskManager_Services.Domains.ServiceFactory;
using TaskManager_Services.Utility;

namespace TaskManager_Services.Domains.Projects
{
    public class ProjectService : IProjectService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;
        private readonly IRepository<Project> _projectRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectService(IServiceFactory serviceFactory, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
            _mapper = _serviceFactory.GetService<IMapper>();
            _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
            _projectRepo = _unitOfWork.GetRepository<Project>();
        }


        public Task<bool> AddTaskToProjectAsync(Guid projectId, Guid taskId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<ProjectDto>> CreateProjectAsync(string userId, ProjectCreateDto reqquest)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    Message = "User not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var project = new Project
            {
                Name = reqquest.Name,
                Description = reqquest.Description
            };

            user.Projects.Add(project);
            await _unitOfWork.SaveChangesAsync();

            var projectDto = new ProjectDto(
                project.Id,
                project.Name,
                project.Description
                );
            return new ServiceResponse<ProjectDto>
            {
                Data = projectDto,
                Message = "Project created successfully",
                StatusCode = HttpStatusCode.Created
            };

        }

        public async Task<ServiceResponse<ProjectDto>> DeleteProjectAsync(Guid projectId, string userId)
        {
            var project = await _projectRepo.GetSingleByAsync(pro => pro.UserId == userId && pro.Id == projectId);
            if (project == null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            await _projectRepo.DeleteAsync(project);

            return new ServiceResponse<ProjectDto>
            {
                Data = null,
                Message = "Project removed",
                StatusCode = HttpStatusCode.NotFound
            };


        }

        public async Task<ServiceResponse<ProjectDto>> DeleteProjectAsyncB(Guid projectId, string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var project = user.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            // Remove the project from the user's projects
            user.Projects.Remove(project);
           
           await _projectRepo.DeleteAsync(project);

           await _unitOfWork.SaveChangesAsync();

            // Map the deleted project to a ProjectDto if needed

            return new ServiceResponse<ProjectDto>
            {
                Data = null,
                Message = "Project deleted successfully",
                StatusCode = HttpStatusCode.OK
            };


        }

        public Task<ServiceResponse<PaginationResponse<ProjectDto>>> GetAllProjectsAsync(RequestParameters requestParameters)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<ProjectDto>> GetProjectAsync(Guid projectId, string userId)
        {
            var project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);
            if (project == null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            if (project.UserId != userId)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Message = "You do not have permission to access this project",
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
            var projectDto = new ProjectDto(
                project.Id,
                project.Name,
                project.Description
                );
            return new ServiceResponse<ProjectDto>
            {
                Data = projectDto,
                Message = "Project retrieved successfully",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<ProjectDto>> GetProjectAsyncB(Guid projectId, string userId)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId && p.UserId == userId);

            if (project is null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Project not found.",
                };
            }
            var projectDto = new ProjectDto(
                project.Id,
                project.Name,
                project.Description
                );
            return new ServiceResponse<ProjectDto>
            {
                Message = $"{project.Name}",
                StatusCode = HttpStatusCode.OK,
                Data = projectDto
            };
        }

        public Task<ServiceResponse<IEnumerable<ProjectDto>>> GetProjectsByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveTaskFromProjectAsync(Guid projectId, Guid taskId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<ProjectDto>> UpdateProjectAsync(Guid projectId, string userId, ProjectUpdateDto request)
        {
            var project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId);
            if (project == null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Message = "Project not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            if (project.UserId != userId)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Message = "You do not have permission to update this project",
                    StatusCode = HttpStatusCode.Forbidden
                };
            }

            project.Name = request.Name;
            project.Description = request.Description;
            
            _projectRepo.Update(project);
            int rows = await _unitOfWork.SaveChangesAsync();

            if (rows > 0)
            {
                var updatedProject = new ProjectDto(
                    project.Id,
                    project.Name,
                    project.Description
                    );
                return new ServiceResponse<ProjectDto>
                {
                    Data = updatedProject,
                    Message = $"{project.Name} updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponse<ProjectDto>
                {
                    Message = "Something went wrong when updating the project",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }



        }
    }
}
