namespace ProjectManagementService.Services.Sprint;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using Dtos.Sprint;
using Task = System.Threading.Tasks.Task;

public sealed class SprintService : ISprintService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SprintService(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
    {
        this._applicationDbContext = applicationDbContext;
        this._httpContextAccessor = httpContextAccessor;
    }

    public async System.Threading.Tasks.Task CreateSprintAsync(CreateSprintDto createSprintDto)
    {
        if(createSprintDto.DateEnd >= createSprintDto.DateStart)
        {
            throw new ArgumentException($"{createSprintDto.DateEnd} cannot be more than or equals to {createSprintDto.DateStart}");
        }

        var projectId = await GetRequestingUsersProjectIdAsync();
        if(await _applicationDbContext.Sprints.AnyAsync(s=>s.ProjectId == projectId 
                                                    && ((s.DateStart == createSprintDto.DateStart
                                                    && s.DateEnd == createSprintDto.DateEnd)
                                                    || createSprintDto.DateStart <= s.DateEnd)))
        {
            throw new ArgumentException($"Sprint with selected dates already exists");
        }

        var sprint = new Sprint
        {
            DateStart = createSprintDto.DateStart,
            DateEnd = createSprintDto.DateEnd,
            Description = createSprintDto.Description,
            ProjectId = projectId.Value
        };

        _applicationDbContext.Sprints.Add(sprint);

        await TrySaveChangesAsync();
    }

    public async System.Threading.Tasks.Task DeleteSprintAsync(Guid sprintId)
    {
        throw new NotImplementedException();
    }

    public async Task<SprintDto> GetSprintAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var currentDate = DateTime.Now;

        var sprint = await _applicationDbContext.Sprints.FirstOrDefaultAsync(
            s => s.ProjectId == projectId
            && s.DateEnd >= currentDate
            && s.DateStart <= currentDate);

        return sprint is null 
            ? new SprintDto(sprint.Id, sprint.DateStart, sprint.DateEnd, sprint.Description)
            : null;
    }

    public async System.Threading.Tasks.Task UpdateSprintAsync()
    {
        throw new NotImplementedException();
    }

    private async System.Threading.Tasks.Task TrySaveChangesAsync()
    {
        try
        {
            await _applicationDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO exception
            throw;
        }
    }

    private async Task<Guid?> GetRequestingUsersProjectIdAsync()
    {
        var identityId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(identityId))
        {
            return null;
        }

        var projectsIdentity = await _applicationDbContext.ProjectsIdentities.FirstOrDefaultAsync(pi => pi.IdentityId == Guid.Parse(identityId));

        return projectsIdentity?.ProjectId;
    }
}
