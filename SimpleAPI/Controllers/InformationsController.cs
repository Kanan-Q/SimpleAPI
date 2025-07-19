using Microsoft.AspNetCore.Mvc;
using SimpleAPI.BL.Cache;
using SimpleAPI.BL.DTO.Information;
using SimpleAPI.Core.Entities;
using SimpleAPI.Core.Repository;
using System.Diagnostics;
using static System.Console;

namespace SimpleAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class InformationsController(IGenericRepository<Information> _repo, ICacheService _cache) : ControllerBase
{
    #region GetAll
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        string cacheKey = "Info_GetAll";
        var sw = Stopwatch.StartNew();
        var cachedData = await _cache.GetAsync<List<Information>>(cacheKey);
        sw.Stop();
        WriteLine($"Time:{sw.ElapsedMilliseconds} ms");
        if (cachedData != null && cachedData.Any()) return Ok(cachedData);
        var data = await _repo.GetAllAsync();
        if (data is null || !data.Any()) return NotFound();
        await _cache.SetAsync(cacheKey, data);
        return Ok(data);
    }
    #endregion GetAll

    #region GetById
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        if (id == 0 || id <= 0) return BadRequest();
        string cacheKey = $"Info_GetById_{id}";
        var cachedItem = await _cache.GetAsync<Information>(cacheKey);
        if (cachedItem != null) return Ok(cachedItem);
        var data = await _repo.GetByIdAsync(id);
        if (data is null) return NotFound();
        await _cache.SetAsync(cacheKey, data);
        return Ok(data);
    }
    #endregion GetById

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(InformationCreateDTO dto)
    {
        if (dto is null || !ModelState.IsValid) return BadRequest();
        Information inf = new()
        {
            CategoryId = dto.CategoryId,
            Description = dto.Description.Trim(),
            Price = dto.Price,
            ProductName = dto.ProductName,
        };
        await _repo.CreateAsync(inf);
        await _cache.RemoveAsync("Info_GetAll");
        return Created($"/api/informations/{inf.Id}", inf);
    }
    #endregion Create

    #region Update
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, InformationUpdateDTO dto)
    {
        if (id == 0 || id <= 0) return BadRequest();
        if (dto is null || !ModelState.IsValid) return BadRequest();
        var data = await _repo.GetByIdAsync(id);
        if (data is null) return BadRequest();
        data.ProductName = dto.ProductName;
        data.CategoryId = dto.CategoryId;
        data.Price = dto.Price;
        data.Description = dto.Description;
        await _repo.UpdateAsync(data);
        await _cache.RemoveAsync("Info_GetAll");
        await _cache.RemoveAsync($"Info_GetById_{id}");
        return NoContent();
    }
    #endregion Update

    #region Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id == 0 || id <= 0) return BadRequest();
        await _repo.DeleteAsync(id);
        await _cache.RemoveAsync("Info_GetAll");
        await _cache.RemoveAsync($"Info_GetById_{id}");
        return NoContent();
    }
    #endregion Delete

    #region BulkInsert
    [HttpPost]
    public async Task<IActionResult> BulkInsert(IEnumerable<InformationCreateDTO> dto)
    {
        if (dto is null || !ModelState.IsValid) return BadRequest();
        var data = new List<Information>();
        foreach (var item in dto)
        {
            Information inf = new()
            {
                CategoryId = item.CategoryId,
                Description = item.Description,
                Price = item.Price,
                ProductName = item.ProductName,
            };
            data.Add(inf);
        }
        await _repo.BulkInsertAsync(data);
        await _cache.RemoveAsync("Info_GetAll");
        return Created("Created", data);
    }
    #endregion BulkInsert

    #region Search
    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest();
        var data = _repo.Search(x => x.ProductName.ToLower().Contains(query) || x.Description.ToLower().Contains(query) || x.Price.ToString().Contains(query) || x.CategoryId.ToString().Contains(query));
        if (data is null) return BadRequest();
        return Ok(data);
    }
    #endregion Search
}

