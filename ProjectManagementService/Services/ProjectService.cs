namespace ProjectManagementService.Services;

using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Data;
using ProjectManagementService.Dtos;
using ProjectManagementService.Models;

public sealed class ProjectService
{
    private readonly ApplicationDbContext _appDbContext;

    public ProjectService(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ProjectDto> GetProjectDataAsync(Guid projectId)
    {
        var project = _appDbContext.Projects.FirstOrDefault(p => p.Id == projectId);
        return new ProjectDto(project);
    }

    public async Task<IEnumerable<DashboardDto>> GetDashboardsAsync(Guid projectID)
    {
        var dashboards = _appDbContext.Dashboards.Where(d => d.Project.Id == projectID);
        var dashboardDtos = dashboards.Select(d => new DashboardDto(d));
        return await dashboardDtos.ToListAsync<DashboardDto>();
    }

    public async Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto)
    {
        var project = await _appDbContext.Projects.FirstOrDefaultAsync(d => d.Id == projectDto.Id);

        // TODO change data to dto

        await _appDbContext.SaveChangesAsync();

        return projectDto;
    }

    public async Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto)
    {
        var newProject = new Project
        {
            // TODO fill in info
        };
        await _appDbContext.AddAsync(newProject);
        await _appDbContext.SaveChangesAsync();
        // TODO check if saved successfully

        return projectDto;
    }

    public async Task<bool> DeleteProjectAsync(Guid projectId)
    {
        var projectToDelete = await _appDbContext.Projects.FirstOrDefaultAsync(p=>p.Id == projectId);
        if(projectToDelete is null)
        {

        }

        var deletedProject = _appDbContext.Projects.Remove(projectToDelete);
        
        // TODO delete all coresponding info, including users, tasks, dashboards and etc
        // Also, log out users, or show them message "project was deleted" slt
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
