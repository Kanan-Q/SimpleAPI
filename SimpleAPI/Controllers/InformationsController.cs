using Microsoft.AspNetCore.Mvc;
using SimpleAPI.BL.DTO.Information;
using SimpleAPI.Core.Entities;
using SimpleAPI.Core.Repository;

namespace SimpleAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InformationsController(IGenericRepository<Information> _repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            if (data is null || !data.Any()) return BadRequest();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data is null) return NotFound();
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
            return Created($"/api/informations/{inf.Id}", inf);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InformationUpdateDTO dto)
        {
            if (dto == null || !ModelState.IsValid) return BadRequest();
            var data = await _repo.GetByIdAsync(id);
            if (data is null) return BadRequest();
            data.ProductName = dto.ProductName;
            data.CategoryId = dto.CateyoryId;
            data.Price = dto.Price;
            data.Description = dto.Description;
            await _repo.UpdateAsync(data);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data is null) return NotFound();
            await _repo.DeleteAsync(id);
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
            return Created("Created",data);
        }

    }
}
