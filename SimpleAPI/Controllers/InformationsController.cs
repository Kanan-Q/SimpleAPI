using Microsoft.AspNetCore.Mvc;
using SimpleAPI.BL.Cache;
using SimpleAPI.BL.DTO.Information;
using SimpleAPI.Core.Entities;
using SimpleAPI.Core.Repository;

namespace SimpleAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InformationsController(IGenericRepository<Information> _repo, ICacheService _cache) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string cacheKey = "Info_GetAll";
            var cachedData = await _cache.GetAsync<List<Information>>(cacheKey);
            if (cachedData != null && cachedData.Any()) return Ok(cachedData);
            var data = await _repo.GetAllAsync();
            if (data is null || !data.Any()) return BadRequest();
            await _cache.SetAsync(cacheKey, data);
            return Ok(data);
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(InformationCreateDTO dto)
        {
            if (dto == null || !ModelState.IsValid) return BadRequest();
            Information inf = new()
            {
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                Price = dto.Price,
                ProductName = dto.ProductName,
            };
            await _repo.CreateAsync(inf);
            await _cache.RemoveAsync("Info_GetAll");
            return Created($"/api/informations/{inf.Id}", inf);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InformationUpdateDTO dto)
        {
            if (id == 0 || id <= 0) return BadRequest();
            if (dto == null || !ModelState.IsValid) return BadRequest();
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0 || id <= 0) return BadRequest();
            var data = await _repo.GetByIdAsync(id);
            if (data is null) return NotFound();
            await _repo.DeleteAsync(id);
            await _cache.RemoveAsync("Info_GetAll");
            await _cache.RemoveAsync($"Info_GetById_{id}");
            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> BulkInsert(IEnumerable<InformationCreateDTO> dto)
        {
            if (dto == null || !ModelState.IsValid) return BadRequest();
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
        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return BadRequest();
            var data = _repo.Search(x => x.ProductName.ToLower().Contains(query) || x.Description.ToLower().Contains(query) || x.Price.ToString().Contains(query) || x.CategoryId.ToString().Contains(query));
            if (data is null) return BadRequest();
            return Ok(data);
        }
    }
}
