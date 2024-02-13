using Microsoft.AspNetCore.Mvc;
using TimeManager.Domain.Context;

namespace TimeManager.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainController(DBContext context) : ControllerBase
{
    private readonly DBContext _context = context;
}