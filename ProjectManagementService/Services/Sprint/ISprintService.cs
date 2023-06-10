namespace ProjectManagementService.Services.Sprint;

using Dtos.Sprint;
using Task = System.Threading.Tasks.Task;
public interface ISprintService
{
    Task CreateSprintAsync(CreateSprintDto createSprintDto);
    Task<SprintDto> GetSprintAsync(Guid sprintId);
    Task<SprintDto?> GetCurrentSprintDtoAsync();
    Task<IEnumerable<SprintDto?>> GetSprintsAsync();
    Task DeleteSprintAsync(Guid? sprintId);
    Task UpdateSprintAsync(UpdateSprintDto updateSprintDto);
    Task FinishSprintAsync();
}
