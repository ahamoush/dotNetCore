using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Validation;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs;
using ProjectManagementSystem.Application.DTOs.Projects;
using ProjectManagementSystem.Application.Interfaces;

namespace ProjectManagementSystem.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;
    private readonly IValidator<ProjectQueryDto> _queryValidator;
    private readonly IValidator<CreateProjectDto> _createValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;

    public ProjectsController(
        IProjectService service,
        IValidator<ProjectQueryDto> queryValidator,
        IValidator<CreateProjectDto> createValidator,
        IValidator<UpdateProjectDto> updateValidator)
    {
        _service = service;
        _queryValidator = queryValidator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [Authorize(Policy = AppPermissions.Projects.View)]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll()
    {
        IEnumerable<ProjectResponseDto> projects = await _service.GetAllAsync();
        return Ok(projects);
    }

    [Authorize(Policy = AppPermissions.Projects.View)]
    [HttpGet]
    public async Task<IActionResult> GetProjects(
        [FromQuery] ProjectQueryDto query)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _queryValidator,
            query,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        PagedResultDto<ProjectResponseDto> result = await _service.GetProjectsAsync(query);
        return Ok(result);
    }

    [Authorize(Policy = AppPermissions.Projects.View)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        ProjectResponseDto? project = await _service.GetByIdAsync(id);
        if (project is null)
            return NotFound(new { message = "Project not found." });

        return Ok(project);
    }

    [Authorize(Policy = AppPermissions.Projects.Create)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _createValidator,
            dto,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        ProjectResponseDto project = await _service.CreateAsync(dto);
        return Ok(project);
    }

    [Authorize(Policy = AppPermissions.Projects.Update)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _updateValidator,
            dto,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        bool updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }

    [Authorize(Policy = AppPermissions.Projects.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }
}
