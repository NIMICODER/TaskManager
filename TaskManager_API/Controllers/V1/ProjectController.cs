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


        //[HttpPost("{userId}")]
        //[ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
        //[ProducesResponseType(400, Type = typeof(ApiResponse))]
        //public async Task<IActionResult> CreateTask([FromRoute] string userId, [FromBody] CreateProjectDto model)
        //{
        //    ServiceResponse<ProjectDto> response = await _projectService.CreateProjectAsync(userId, model);
        //    return ComputeResponse(response);
        //}
        [HttpGet]
        public IActionResult GetProjects()
        {
            return Ok("OK");
        }

        [HttpGet("{id}")]
        public IActionResult GetProject(Guid id)
        {
            return Ok("OK");
        }

        [HttpPost("{taskId}/assign-to-project/{projectId}")]
        public IActionResult AssignTaskToProject(Guid taskId, Guid projectId)
        {
            return Ok("OK");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(Guid id, [FromBody] ProjectDto projectDto)
        {
            return Ok("OK");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(Guid id)
        {
            return Ok("OK");
        }
        [HttpDelete("{taskId}/remove-from-project")]
        public IActionResult RemoveTaskFromProject(Guid taskId)
        {
            return Ok("OK");
        }


    }
}
