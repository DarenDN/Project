namespace ProjectManagementService.Services.Sprint;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using Dtos.Sprint;
using Task = System.Threading.Tasks.Task;
using System.Collections.Generic;

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
        if(createSprintDto.DateEnd <= createSprintDto.DateStart)
        {
            throw new ArgumentException($"{createSprintDto.DateEnd} cannot be more than or equals to {createSprintDto.DateStart}");
        }
        var dateStart = createSprintDto.DateStart.Date;
        var dateEnd = createSprintDto.DateEnd.Date;
        var projectId = await GetRequestingUsersProjectIdAsync();
        if(await _applicationDbContext.Sprints.AnyAsync(s=>s.ProjectId == projectId 
                                                    && ((s.DateStart == dateStart
                                                    && s.DateEnd == dateEnd)
                                                    || dateStart <= s.DateEnd)))
        {
            throw new ArgumentException($"Sprint with selected dates already exists");
        }

        var sprint = new Sprint
        {
            DateStart = dateStart,
            DateEnd = dateEnd,
            Description = createSprintDto.Description,
            Name = !string.IsNullOrWhiteSpace(createSprintDto.Name)
                        ? createSprintDto.Name
                        : $"{dateStart.ToShortDateString()} - {dateEnd.ToShortDateString()}",
            ProjectId = projectId.Value
        };

        _applicationDbContext.Sprints.Add(sprint);

        await TrySaveChangesAsync();
    }

    public async System.Threading.Tasks.Task DeleteSprintAsync(Guid? sprintId)
    {
        throw new NotImplementedException();
    }

    public async System.Threading.Tasks.Task UpdateSprintAsync(UpdateSprintDto updateSprintDto)
    {
        var sprint = await _applicationDbContext.Sprints.FirstOrDefaultAsync(
            s => s.Id == updateSprintDto.SprintId);

        sprint.Name = updateSprintDto.Title;
        sprint.Description = updateSprintDto.Description;
        _applicationDbContext.Sprints.Update(sprint);
        await TrySaveChangesAsync();
    }

    public async Task<SprintDto> GetSprintAsync(Guid sprintId)
    {
        var sprint = await _applicationDbContext.Sprints.FirstOrDefaultAsync(
            s => s.Id == sprintId);

        if(sprint is null)
        {
            throw new ArgumentException(nameof(sprintId));
        }

        return new SprintDto(sprint.Id, sprint.Name, sprint.DateStart, sprint.DateEnd, sprint.Description);
    }

    public async Task<SprintDto?> GetCurrentSprintAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var currentDate = DateTime.Now;

        var sprint = await _applicationDbContext.Sprints.FirstOrDefaultAsync(
            s => s.ProjectId == projectId
            && s.DateEnd >= currentDate
            && s.DateStart <= currentDate);

        return sprint is not null
            ? new SprintDto(sprint.Id, sprint.Name, sprint.DateStart, sprint.DateEnd, sprint.Description)
            : null;
    }

    public async Task<IEnumerable<SprintDto?>> GetSprintsAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();

        return await _applicationDbContext.Sprints
            .Where(s => s.ProjectId == projectId)
            .Select(s => new SprintDto(s.Id, s.Name, s.DateStart, s.DateEnd, s.Description))
            .ToListAsync();
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
