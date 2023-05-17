namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos.Project;
using Services.Project;

// working with project could be easily drawn into one own microservice
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    [Route(nameof(GetProjectDataAsync))]
    public async Task<ActionResult> GetProjectDataAsync()
    {
        try
        {
            var projectData = await _projectService.GetProjectAsync();
            return Ok(projectData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(UpdateProjectAsync))]
    public async Task<ActionResult> UpdateProjectAsync(UpdateProjectDto projectDto)
    {
        try
        {
            await _projectService.UpdateProjectAsync(projectDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(CreateProjectAsync))]
    public async Task<ActionResult> CreateProjectAsync(CreateProjectDto projectDto)
    {
        try
        {
            await _projectService.CreateProjectAsync(projectDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    [Route(nameof(DeleteProjectAsync))]
    public async Task<ActionResult> DeleteProjectAsync()
    {
        try
        {
            await _projectService.DeleteProjectAsync();
            return Ok();
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
