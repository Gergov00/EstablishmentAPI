using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstablishmentAPI.Data;
using EstablishmentAPI.Models;
using EstablishmentAPI.DTOs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstablishmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(AppDbContext context, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Категория с ID {Id} не найдена.", id);
                return NotFound();
            }

            return _mapper.Map<CategoryDTO>(category);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CreateCategoryDTO createDto)
        {
            _logger.LogInformation("Создаётся новая категория: {Name}", createDto.Name);

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                                                        .SelectMany(v => v.Errors)
                                                        .Select(e => e.ErrorMessage));
                _logger.LogWarning("Модель не валидна: {Errors}", errors);
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(createDto);
            _context.Categories.Add(category);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Категория с ID {Id} успешно создана.", category.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при создании категории: {Name}", createDto.Name);
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }

            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CreateCategoryDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                                                        .SelectMany(v => v.Errors)
                                                        .Select(e => e.ErrorMessage));
                _logger.LogWarning("Модель не валидна: {Errors}", errors);
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Категория с ID {Id} не найдена для обновления.", id);
                return NotFound();
            }

            _mapper.Map(updateDto, category);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Категория с ID {Id} успешно обновлена.", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении категории с ID {Id}.", id);
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }

            return NoContent();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Категория с ID {Id} не найдена для удаления.", id);
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Категория с ID {Id} успешно удалена.", id);
            return NoContent();
        }
    }
}
