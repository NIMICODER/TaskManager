using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Controllers.V1.Shared;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.Projects;
using TaskManager_Services.Domains.Projects.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpPost("{userId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> CreateProject([FromRoute] string userId, [FromBody] ProjectCreateDto model)
        {
            ServiceResponse<ProjectDto> response = await _projectService.CreateProjectAsync(userId, model);
            return ComputeResponse(response);
        }

        [HttpPut("update/{userId}/{projectId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> UpdateProject([FromRoute] string userId, [FromRoute] Guid projectId, [FromBody] ProjectUpdateDto model)
        {
            ServiceResponse<ProjectDto> response = await _projectService.UpdateProjectAsync(projectId, userId,  model);
            return ComputeResponse(response);
        }

        [HttpDelete("delete/{userId}/{projectId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> DeleteProject([FromRoute] string userId, [FromRoute] Guid projectId)
        {
            ServiceResponse<ProjectDto> response = await _projectService.DeleteProjectAsync( projectId, userId);
            return ComputeResponse(response);
        }



    }
}
