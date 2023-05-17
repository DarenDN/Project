namespace ProjectManagementService.Services.Task;
using Dtos.Task;
using Task = System.Threading.Tasks.Task;

public interface ITaskService
{
    Task CreateTaskAsync(CreateTaskDto newTaskDto);
    Task UpdateTaskAsync(TaskUpdateDto taskDto);
    Task DeleteTaskAsync(Guid taskId);
    Task<TaskDataDto> GetTaskAsync(Guid taskId);
    Task<IEnumerable<TaskShortInfoDto>> GetTasksAsync(Guid dashboardId);

    Task ChangeStatusAsync(Guid taskId, Guid statusId);

    Task ChangeTypeAsync(Guid taskId, Guid typeId);

    Task ChangePerformerAsync(Guid taskId, Guid? performerId);
    // TODO set sprint
    // TODO set story
}
