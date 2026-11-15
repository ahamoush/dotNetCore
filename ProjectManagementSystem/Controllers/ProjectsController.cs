using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.DTOs.Projects;
using ProjectManagementSystem.Interfaces;

namespace ProjectManagementSystem.Controllers;

// ApiController: يفعّل ميزات الـ API مثل Validation التلقائي
[ApiController]
// المسار الأساسي: /api/projects
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    // Dependency Injection: ASP.NET يمرر IProjectService تلقائياً
    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    // GET /api/projects/all
    // يرجع كل المشاريع بدون Pagination
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll()
    {
        var projects = await _service.GetAllAsync();
        return Ok(projects); // 200 OK
    }

    // GET /api/projects?search=web&status=1&pageNumber=1&pageSize=10&sortBy=name&descending=false
    // بحث + فلترة + ترتيب + صفحات
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ProjectResponseDto>>> GetProjects(
        [FromQuery] ProjectQueryDto query)
    {
        // تحقق بسيط من معاملات الصفحات
        if (query.PageNumber < 1 ||
            query.PageSize < 1 ||
            query.PageSize > 100)
        {
            return BadRequest(new { message = "Invalid pagination parameters." });
        }

        var result = await _service.GetProjectsAsync(query);
        return Ok(result);
    }

    // GET /api/projects/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);

        if (project is null)
            return NotFound(new { message = "Project not found." }); // 404

        return Ok(project);
    }

    // POST /api/projects
    // Body مثال:
    // { "name": "My Project", "description": "Learning ASP.NET Core" }
    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(CreateProjectDto dto)
    {
        var project = await _service.CreateAsync(dto);

        // 201 Created + رابط للمورد الجديد في Header اسمه Location
        return CreatedAtAction(
            nameof(GetById),
            new { id = project.Id },
            project
        );
    }

    // PUT /api/projects/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(new { message = "Project not found." });

        return NoContent(); // 204: نجح التعديل بدون Body
    }

    // DELETE /api/projects/5
    // ملاحظة: هذا Soft Delete (المشروع يختفي من النتائج لكنه يفضل في الداتابيز)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Project not found." });

        return NoContent();
    }
}
