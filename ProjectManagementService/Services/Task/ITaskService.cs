namespace ProjectManagementService.Services.Task;
using Dtos.Task;
using ProjectManagementService.Dtos.Estimation;
using Task = System.Threading.Tasks.Task;

public interface ITaskService
{
    Task CreateTaskAsync(CreateTaskDto newTaskDto);
    Task UpdateTaskAsync(TaskUpdateDto taskDto);
    Task DeleteTaskAsync(Guid taskId);
    Task<Dictionary<Guid, int>> GetTasksBacklogAsync();
    Task UpdateTasksAsync(IEnumerable<TaskSprintEvaluationInfo> taskSprintInfos);
    Task<TaskDataDto> GetTaskAsync(Guid taskId);
    Task<IEnumerable<TaskShortInfoDto>> GetTasksAsync();
    Task<IEnumerable<TaskShortInfoDto>> GetSprintTasksAsync();
    Task<IEnumerable<TaskShortInfoDto>> GetTasksWithStatesAsync(List<Guid>? states);

    Task ChangeStateAsync(Guid taskId, Guid statusId);

    Task ChangeTypeAsync(Guid taskId, Guid typeId);

    Task ChangePerformerAsync(Guid taskId, Guid? performerId);

    Task SetEstimationManyAsync(List<EstimationDto> estimationDtos);

    Task SetEstimationSingleAsync(EstimationDto estimationDto);

    Task SetStoryManyAsync(List<Guid> taskIds);

    Task SetStorySingleAsync(Guid taskId);
    // TODO set sprint
    // TODO set story
}
