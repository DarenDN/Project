namespace ProjectManagementService.Dtos;

using ProjectManagementService.Models;

public sealed class ProjectDto
{
    public ProjectDto(Project project) 
    {
        Id = project.Id;
        Title = project.Title;
        // TODO something else
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
}
