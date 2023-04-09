namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos;
using Services;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    [Route(nameof(GetProjectDataAsync))]
    public async Task<JsonResult> GetProjectDataAsync(Guid projectId)
    {
        var projectData = await _projectService.GetProjectDataAsync(projectId);

        return new JsonResult(projectData);
    }

    [HttpGet]
    [Route(nameof(GetDashboardsAsync))]
    public async Task<JsonResult> GetDashboardsAsync(Guid projectId)
    {
        var dashboards = await _projectService.GetDashboardsAsync(projectId);

        return new JsonResult(dashboards);
    }

    [HttpPut]
    [Route(nameof(UpdateProjectAsync))]
    public async Task<JsonResult> UpdateProjectAsync(ProjectDto projectDto)
    {
        var updatedProject = await _projectService.UpdateProjectAsync(projectDto);
        return new JsonResult(updatedProject);
    }

    [HttpPost]
    [Route(nameof(CreateProjectAsync))]
    public async Task<JsonResult> CreateProjectAsync(ProjectDto projectDto)
    {
        var createdProject = await _projectService.CreateProjectAsync(projectDto);
        return new JsonResult(1);
    }

    [HttpDelete]
    [Route(nameof(DeleteProjectAsync))]
    public async Task<ActionResult> DeleteProjectAsync(Guid projectId)
    {
        var success = await _projectService.DeleteProjectAsync(projectId);
        // TODO if delete greate if not then something else 
        return Ok();
    }
}
