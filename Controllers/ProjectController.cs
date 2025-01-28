using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models;

[Route("api/projects")]
[ApiController]
public class ProjectsController : ControllerBase
{
    // Endpoint for Admins to create a project
    [Authorize(Roles = "Admin")]
    [HttpPost("create-project")]
    public IActionResult CreateProject([FromBody] ProjectModel model)
    {
        // Logic for project creation
        return Ok(new { Message = "Project created successfully." });
    }

    // Endpoint for Team Members to view projects
    [Authorize(Roles = "TeamMember")]
    [HttpGet("view-projects")]
    public IActionResult ViewProjects()
    {
        // Logic to retrieve and return projects
        return Ok(new { Message = "List of projects." });
    }
}