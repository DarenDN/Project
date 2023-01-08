namespace ProjectManagementService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dtos;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    [HttpPost()]
    [Route(nameof(GetTask))]
    public async Task<JsonResult> GetTask(TaskDto taskDto)
    {
        // TODO return either Dto or Model itself, or I can fill up the dto that we getting and send it back
        var taskData = await GetTaskData(taskDto);
        return new JsonResult(taskData);
    }

    private async Task<TaskDto> GetTaskData(TaskDto taskDto)
    {
        return new TaskDto(new TaskModel());
    }
}
