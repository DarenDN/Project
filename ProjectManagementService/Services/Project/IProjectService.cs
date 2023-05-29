namespace ProjectManagementService.Services.Project;
using Dtos.Project;
using System.Threading.Tasks;

public interface IProjectService
{
    Task SetProjectIdentityAsync(Guid identityId);
    Task DeleteProjectIdentityAsync(Guid identityId);

    Task<ProjectDto> GetProjectAsync();

    Task<CreatedProjectDto> CreateProjectAsync(CreateProjectDto projectDto);

    Task DeleteProjectAsync();

    Task UpdateProjectAsync(UpdateProjectDto projectId);
}
