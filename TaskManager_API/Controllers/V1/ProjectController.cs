using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Controllers.V1.Shared;
using TaskManager_Services.Domains.Projects.Dtos;

namespace TaskManager_API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : BaseController
    {


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

        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectDto projectDto)
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
