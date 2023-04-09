namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos;
using ProjectManagementService.Services;

[Route("api/[controller]")]
[ApiController]
public sealed class TaskController : ControllerBase
{
    private TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost()]
    [Route(nameof(CreateTaskAsync))]
    public async Task<JsonResult> CreateTaskAsync(TaskDto taskDto)
    {
        // TODO return either Dto or Model itself, or I can fill up the dto that we getting and send it back
        var taskData = await _taskService.CreateTaskAsync(taskDto);
        return new JsonResult(taskData);
    }

    [HttpPut()]
    [Route(nameof(CreateTaskAsync))]
    public async Task<JsonResult> UpdateTaskAsync(TaskDto taskDto)
    {
        // TODO return either Dto or Model itself, or I can fill up the dto that we getting and send it back
        var taskData = await _taskService.UpdateTaskAsync(taskDto);
        return new JsonResult(taskData);
    }

    [HttpDelete()]
    [Route(nameof(DeleteTaskAsync))]
    public async Task<ActionResult> DeleteTaskAsync(Guid taskId)
    {
        // TODO return either Dto or Model itself, or I can fill up the dto that we getting and send it back
        var result = await _taskService.DeleteTaskAsync(taskId);
        return Ok();
    }
}
