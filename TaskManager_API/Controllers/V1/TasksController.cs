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


        
        [HttpGet("{taskId}")]
        [SwaggerOperation(Summary = "Gets taskitem with id")]
        [ProducesResponseType(200, Type = typeof(ApiRecordResponse<TaskDto>))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> GetTask(Guid taskId)
        {
            var response = await _taskService.GetTaskAsync(taskId);
            return ComputeResponse(response);
        }

        //[HttpPost("create-task")]
        //[ProducesResponseType(200, Type = typeof(ApiRecordResponse<TaskDto>))]
        //[ProducesResponseType(404, Type = typeof(ApiResponse))]
        //[ProducesResponseType(400, Type = typeof(ApiResponse))]
        //public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto model)
        //{
        //   var response = await _taskService.CreateTaskAsync(model);
        //    return ComputeResponse(response);
        //}

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto model, Guid projectId)
        {
            ServiceResponse<TaskDto> response = await _taskService.CreateTaskAsync(projectId, model);
            return ComputeResponse(response);
        }


        [HttpPut("update/{projectId}/{taskId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] TaskUpdateDto model)
        {
            ServiceResponse<TaskDto> response = await _taskService.UpdateTaskAsync(taskId, projectId, model);
            return ComputeResponse(response);
        }

        [HttpPut("{projectId}/assign")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> AssignTaskToProject([FromRoute] Guid taskId, [FromRoute] Guid projectId)
        {
            ServiceResponse<TaskDto> response = await _taskService.AssignTaskToProjectAsync(taskId, projectId);    
            return ComputeResponse(response);
        }
        [HttpPut("{projectId}/assign/{taskId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> RemoveTaskFromAProject( [FromRoute] Guid taskId, [FromRoute] Guid projectId)
        {
            ServiceResponse<TaskDto> response = await _taskService.RemoveTaskFromProjectAsync(taskId, projectId);   
            return ComputeResponse(response);
        }

        [HttpGet("due/{projectId}")]
        [ProducesResponseType(200, Type = typeof(ApiRecordResponse<PaginationResponse<TaskDto>>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> GetTasksByDueDateForTheWeek([FromRoute] Guid projectId, [FromQuery] RequestParameters requestParameters)
        {
            ServiceResponse<PaginationResponse<TaskDto>> response = await _taskService.GetTasksDueThisWeekAsync(projectId, requestParameters);  
            return ComputeResponse(response);
        }

        [HttpPut("status/{taskId}/{userId}/{status}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> ChangeTaskStatus([FromRoute] Guid taskId, [FromRoute] string userId, [FromRoute] TasksStatus status)
        {
            ServiceResponse<TaskDto> response = await _taskService.ToggleTaskStatusAsync(taskId, userId, status);
            return ComputeResponse(response);
        }

        [HttpDelete("delete/{taskId}/{projectId}")]
        
        [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse))]
        public async Task<IActionResult> DeleteTask( [FromRoute] Guid taskId, [FromRoute] Guid projectId)
        {
            ServiceResponse<TaskDto> response = await _taskService.DeleteTaskAsync(taskId, projectId);
            return ComputeResponse(response);
        }

    }
}
