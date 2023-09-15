using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager_API.Controllers.V1.Shared;
using TaskManager_Models.Entities.Enums;
using TaskManager_Models.Utility;
using TaskManager_Services.Domains.Tasks;
using TaskManager_Services.Domains.Tasks.Dtos;
using TaskManager_Services.Utility;

namespace TaskManager_API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok("OK");
        }

        [HttpGet("{taskId}")]
        [SwaggerOperation(Summary = "Gets taskitem with id")]
        [ProducesResponseType(200, Type = typeof(ApiRecordResponse<TaskDto>))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var response = await _taskService.GetTaskAsync(id);
            return ComputeResponse(response);
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


        [HttpPost("create-task")]
        [ProducesResponseType(200, Type = typeof(ApiRecordResponse<TaskDto>))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto model)
        {
           var response = await _taskService.CreateTaskAsync(model);
            return ComputeResponse(response);
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
