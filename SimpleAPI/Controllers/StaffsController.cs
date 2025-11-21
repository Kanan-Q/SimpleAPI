using Microsoft.AspNetCore.Mvc;
using SimpleAPI.Core.Cache;
using SimpleAPI.Core.Entities;
using SimpleAPI.Core.Repository;

namespace SimpleAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]

public class StaffsController(IGenericRepository<Staff> _repo, ICacheService _cache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _repo.GetAllAsync();
        if (data is null || !data.Any()) return BadRequest();
        return Ok(data);
    }
}
