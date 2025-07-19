using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAPI.BL.Cache;
using SimpleAPI.Core.Entities;
using SimpleAPI.Core.Repository;

namespace SimpleAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class CategoriesController(IGenericRepository<Category> _repo, ICacheService _cache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        string cacheKey = "Categories_GetAll";
        var cacheData = await _cache.GetAsync<List<Category>>(cacheKey);
        if (cacheData != null && cacheData.Any()) return Ok(cacheData);
        var data = await _repo.GetAllAsync();
        if (data is null || !data.Any()) return NotFound();
        await _cache.SetAsync(cacheKey, cacheData);
        return Ok(data);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        string cacheKey = $"Categories_Get_By_Id{id}";
        var cacheData = await _cache.GetAsync<List<Category>>(cacheKey);
        if (cacheData != null && cacheData.Any()) return Ok(cacheData);
        var data = await _repo.GetByIdAsync(id);
        if (data is null) return NotFound();
        await _cache.SetAsync(cacheKey, cacheData);
        return Ok(data);
    }
}
