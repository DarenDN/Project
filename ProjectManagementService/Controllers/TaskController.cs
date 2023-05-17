namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos.Task;
using Services.Task;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]"), Authorize]
[ApiController]
public sealed class TaskController : ControllerBase
{
    private ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    [Route(nameof(CreateTaskAsync))]
    public async Task<ActionResult> CreateTaskAsync(CreateTaskDto taskDto)
    {
        try
        {
            await _taskService.CreateTaskAsync(taskDto);
            return Ok();
        }
        catch (Exception ex) 
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(UpdateTaskAsync))]
    public async Task<ActionResult> UpdateTaskAsync(TaskUpdateDto taskDto)
    {
        try
        {
            await _taskService.UpdateTaskAsync(taskDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    [Route(nameof(DeleteTaskAsync))]
    public async Task<ActionResult> DeleteTaskAsync(Guid taskId)
    {
        try
        {
            await _taskService.DeleteTaskAsync(taskId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(GetTaskAsync))]
    public async Task<ActionResult> GetTaskAsync(Guid taskId)
    {
        try
        {
            var task = await _taskService.GetTaskAsync(taskId); 
            return Ok(task);
        }
        catch (Exception ex) 
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(GetTasksAsync))]
    public async Task<ActionResult> GetTasksAsync(Guid dashboardId)
    {
        try
        {
            var task = await _taskService.GetTasksAsync(dashboardId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPut]
    [Route(nameof(ChangeStatusAsync))]
    public async Task<ActionResult> ChangeStatusAsync(Guid taskId, Guid statusId)
    {
        try
        {
            await _taskService.ChangeStatusAsync(taskId, statusId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(ChangeTypeAsync))]
    public async Task<ActionResult> ChangeTypeAsync(Guid taskId, Guid typeId)
    {
        try
        {
            await _taskService.ChangeTypeAsync(taskId, typeId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(ChangePerformerAsync))]
    public async Task<ActionResult> ChangePerformerAsync(Guid taskId, Guid? performerId)
    {
        try
        {
            await _taskService.ChangePerformerAsync(taskId, performerId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
