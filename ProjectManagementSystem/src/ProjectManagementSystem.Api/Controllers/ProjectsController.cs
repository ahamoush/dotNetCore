using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs;
using ProjectManagementSystem.Application.DTOs.Projects;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Application.Validators;

namespace ProjectManagementSystem.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [Authorize(Policy = AppPermissions.Projects.View)]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll()
    {
        var projects = await _service.GetAllAsync();
        return Ok(projects);
    }

    [Authorize(Policy = AppPermissions.Projects.View)]
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ProjectResponseDto>>> GetProjects(
        [FromQuery] ProjectQueryDto query)
    {
        if (!ProjectQueryValidator.HasValidPaging(query))
            return BadRequest(new { message = "Invalid pagination parameters." });

        var result = await _service.GetProjectsAsync(query);
        return Ok(result);
    }

    [Authorize(Policy = AppPermissions.Projects.View)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);
        if (project is null)
            return NotFound(new { message = "Project not found." });

        return Ok(project);
    }

    [Authorize(Policy = AppPermissions.Projects.Create)]
    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(CreateProjectDto dto)
    {
        var project = await _service.CreateAsync(dto);
        return Ok(project);
    }

    [Authorize(Policy = AppPermissions.Projects.Update)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }

    [Authorize(Policy = AppPermissions.Projects.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }
}
