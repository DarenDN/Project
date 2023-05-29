namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos.Project;
using Services.Project;
using Microsoft.AspNetCore.Authorization;

// working with project could be easily drawn into one own microservice
[ApiController, Authorize]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    [Route(nameof(SetProjectIdentityAsync))]
    public async Task<ActionResult> SetProjectIdentityAsync(Guid identityId)
    {
        try
        {
            await _projectService.SetProjectIdentityAsync(identityId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    [Route(nameof(DeleteProjectIdentityAsync))]
    public async Task<ActionResult> DeleteProjectIdentityAsync(Guid identityId)
    {
        try
        {
            await _projectService.DeleteProjectIdentityAsync(identityId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
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
            var createdProjectDto = await _projectService.CreateProjectAsync(projectDto);
            return Ok(createdProjectDto);
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
