namespace ProjectManagementService.Services.Project;
using Dtos.Project;
using System.Threading.Tasks;

public interface IProjectService
{
    Task<ProjectDto> GetProjectAsync();

    Task CreateProjectAsync(CreateProjectDto projectDto);

    Task DeleteProjectAsync();

    Task UpdateProjectAsync(UpdateProjectDto projectId);
}
