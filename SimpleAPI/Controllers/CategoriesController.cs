using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAPI.BL.Cache;
using SimpleAPI.BL.DTO.Category;
using SimpleAPI.Core.Entities;
using SimpleAPI.Core.Repository;

namespace SimpleAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class CategoriesController(IGenericRepository<Category> _repo, ICacheService _cache) : ControllerBase
{
    #region GetAll
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
    #endregion GetAll

    #region GetById
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        string cacheKey = $"Categories_GetById{id}";
        var cacheData = await _cache.GetAsync<List<Category>>(cacheKey);
        if (cacheData != null && cacheData.Any()) return Ok(cacheData);
        var data = await _repo.GetByIdAsync(id);
        if (data is null) return NotFound();
        await _cache.SetAsync(cacheKey, cacheData);
        return Ok(data);
    }
    #endregion GetById

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDTO dto)
    {
        if (dto is null || !ModelState.IsValid) return BadRequest();
        Category category = new()
        {
            CategoryName = dto.CategoryName
        };
        await _repo.CreateAsync(category);
        await _cache.RemoveAsync("Categories_GetAll");
        return Created($"api/Categories/{category.Id}:", category);
    }
    #endregion Create

    #region Update
    [HttpPut]
    public async Task<IActionResult> Update(int id, CategoryUpdateDTO dto)
    {
        if (dto is null || !ModelState.IsValid) return BadRequest();
        var data = await _repo.GetByIdAsync(id);
        if (data is null) return BadRequest();
        data.CategoryName = dto.CategoryName;
        await _repo.UpdateAsync(data);
        await _cache.RemoveAsync("Categories_GetAll");
        await _cache.RemoveAsync($"Categories_GetById{id}");
        return NoContent();
    }
    #endregion Update

    #region Delete
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        if (id == null) return BadRequest();
        await _repo.DeleteAsync(id);
        await _cache.RemoveAsync("Categories_GetAll");
        await _cache.RemoveAsync($"Categories_GetById{id}");
        return NoContent();
    }
    #endregion Delete

}
