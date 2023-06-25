namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos.Task;
using Services.Task;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementService.Dtos.Estimation;
using Microsoft.AspNetCore.Cors;
using ProjectManagementService.Dtos.Backlog;

[Route("api/[controller]"), Authorize]
[EnableCors("CorsPolicy")]
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
    [Route(nameof(UpdateTasksAsync))]
    public async Task<ActionResult> UpdateTasksAsync(MeetingResultDto meetingResultDto)
    {
        try
        {
            await _taskService.UpdateTasksAsync(meetingResultDto.BacklogTaskDtos);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message + ex.StackTrace);
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
            return new JsonResult(task);
        }
        catch (Exception ex) 
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(GetTasksBacklogAsync))]
    public async Task<ActionResult> GetTasksBacklogAsync()
    {
        try
        {
            var tasksBacklog = await _taskService.GetTasksBacklogAsync();
            return new JsonResult( new { tasksBacklog });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(GetSprintTasksAsync))]
    public async Task<ActionResult> GetSprintTasksAsync()
    {
        try
        {
            var tasks = await _taskService.GetSprintTasksAsync();
            return new JsonResult(new { tasks });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(GetTasksAsync))]
    public async Task<ActionResult> GetTasksAsync()
    {
        try
        {
            var tasks = await _taskService.GetTasksAsync();
            return new JsonResult(new { tasks });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(GetTasksWithStatesAsync))]
    public async Task<ActionResult> GetTasksWithStatesAsync(List<Guid>? states)
    {
        try
        {
            var tasks = await _taskService.GetTasksWithStatesAsync(states);
            return new JsonResult( new { tasks });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(ChangeStateAsync))]
    public async Task<ActionResult> ChangeStateAsync(Guid taskId, Guid statusId)
    {
        try
        {
            await _taskService.ChangeStateAsync(taskId, statusId);
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

    [HttpPut]
    [Route(nameof(SetCurrentUserAsPerformerAsync))]
    public async Task<ActionResult> SetCurrentUserAsPerformerAsync(Guid taskId)
    {
        try
        {
            await _taskService.SetCurrentUserAsPerformerAsync(taskId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(SetEstimationManyAsync))]
    public async Task<ActionResult> SetEstimationManyAsync(List<EstimationDto> estimationDtos)
    {
        try
        {
            await _taskService.SetEstimationManyAsync(estimationDtos);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(SetEstimationSingleAsync))]
    public async Task<ActionResult> SetEstimationSingleAsync(EstimationDto estimationDto)
    {
        try
        {
            await _taskService.SetEstimationSingleAsync(estimationDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(SetStoryManyAsync))]
    public async Task<ActionResult> SetStoryManyAsync(List<Guid> taskIds)
    {
        try
        {
            await _taskService.SetStoryManyAsync(taskIds);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(SetStorySingleAsync))]
    public async Task<ActionResult> SetStorySingleAsync(Guid taskIds)
    {
        try
        {
            await _taskService.SetStorySingleAsync(taskIds);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
