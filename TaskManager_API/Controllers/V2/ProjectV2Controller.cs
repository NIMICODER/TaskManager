using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.Projects;
using TaskManager_Services.Domains.Projects.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_API.Controllers.V2
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectV2Controller : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectV2Controller(IProjectService projectService)
        {
            _projectService = projectService;
                
        }

        [HttpPost("{userId}")]
      
        public async Task<IActionResult> CreateProject([FromRoute] string userId, [FromBody] ProjectCreateDto model)
        {
            var response = await _projectService.CreateProjectAsync(userId, model);
            return Ok(response);
        }
    }
}
