using ProjectManagementService.Dtos.State;

namespace ProjectManagementService.Services.State;

public interface IStateService
{
    Task<IEnumerable<StateDto>> GetStatesAsync();
    Task<IEnumerable<StateDto>> GetNextStatesAsync(Guid currentState);

}
