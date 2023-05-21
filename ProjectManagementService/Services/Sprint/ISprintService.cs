namespace ProjectManagementService.Services.Sprint;

using Dtos.Sprint;
using Task = System.Threading.Tasks.Task;
public interface ISprintService
{
    Task CreateSprintAsync(CreateSprintDto createSprintDto);
    Task<SprintDto> GetSprintAsync();
    Task DeleteSprintAsync(Guid sprintId);
    Task UpdateSprintAsync();
}
