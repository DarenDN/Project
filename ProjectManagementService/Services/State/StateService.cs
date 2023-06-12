namespace ProjectManagementService.Services.State;
using Microsoft.EntityFrameworkCore;
using Data;
using Dtos.State;
public class StateService : IStateService
{
    private readonly Data.AppDbContext _applicationDbContext;

    public StateService(Data.AppDbContext applicationDbContext)
    {
        this._applicationDbContext = applicationDbContext;
    }

    public async Task<IEnumerable<StateDto>> GetNextStatesAsync(Guid currentState)
    {
        var nextStates = await _applicationDbContext.StateRelationships
            .Where(sr => sr.StateCurrent == currentState)
            .Join(_applicationDbContext.TaskStates,
                    sr => sr.StateNext,
                    s => s.Id,
                    (sr, s) => s)
            .Select(s => new StateDto(s.Id, s.Name, s.Order))
            .ToListAsync();

        return nextStates;
    }

    public async Task<IEnumerable<StateDto>> GetStatesAsync()
    {
        var states = await _applicationDbContext.TaskStates
            .Select(s => new StateDto(s.Id, s.Name, s.Order)).ToListAsync();
        return states;
    }
}
