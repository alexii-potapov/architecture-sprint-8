using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reports.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "First", "Second", "Third", "Fourth", "Fifth"
    ];

    [HttpGet]
    public IActionResult Get()
    {
        // Получаем claims пользователя из токена
        string? userId = User.FindFirst("preferred_username")?.Value;
        string? userName = User.FindFirst("name")?.Value;
        string? userRoles = User.FindFirst("realm_access")?.Value;

        System.Console.WriteLine($"Request from UserId = {userId}, Name = {userName}");
        
        RealmAccess? realmAccess = JsonSerializer.Deserialize<RealmAccess>(userRoles);
        List<string> roles = realmAccess?.Roles ?? new List<string>();
       
        if (!roles.Contains("prothetic_user"))
        {   
            System.Console.WriteLine("Not prothetic_user");

            return Unauthorized();
        }
        
        Report[] report = Enumerable.Range(1, 5).Select(index => new Report
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-index)),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        
        return Ok(report);
    }
}