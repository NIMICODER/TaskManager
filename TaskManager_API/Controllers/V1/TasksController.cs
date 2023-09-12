using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Controllers.V1.Shared;
using TaskManager_Models.Entities.Enums;
using TaskManager_Services.Domains.Tasks.Dtos;

namespace TaskManager_API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController
    {

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok("OK");
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(Guid id)
        {
            return Ok("OK");
        }

        [HttpGet("due-this-week")]
        public IActionResult GetTasksDueThisWeek()
        {
            return Ok("OK");
        }

        [HttpGet("filter")]
        public IActionResult GetTasksByStatusOrPriority(TaskStatus? status = null, TaskPriority? priority = null)
        {
            return Ok("OK");
        }


        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskDto taskDto)
        {
            return Ok("OK");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] TaskDto taskDto)
        {
            return Ok("OK");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            return Ok("OK");
        }
    }
}
