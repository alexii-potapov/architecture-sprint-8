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
    public IEnumerable<Report> Get()
    {
        // Получаем claims пользователя из токена
        string? userId = User.FindFirst("sub")?.Value;
        string? userName = User.FindFirst("preferred_username")?.Value;
        System.Console.WriteLine($"Request from UserId = {userId}, Name = {userName}");

        return Enumerable.Range(1, 5).Select(index => new Report
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-index)),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
