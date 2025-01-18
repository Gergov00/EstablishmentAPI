// Controllers/EstablishmentsController.cs
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstablishmentAPI.Data;
using EstablishmentAPI.Models;
using EstablishmentAPI.Extensions;
using EstablishmentAPI.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstablishmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablishmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EstablishmentsController> _logger;

        public EstablishmentsController(AppDbContext context, IMapper mapper, ILogger<EstablishmentsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // POST: api/Establishments
        [HttpPost]
        public async Task<ActionResult<EstablishmentDTO>> PostEstablishment(CreateEstablishmentDTO createDto)
        {
            _logger.LogInformation("Создаётся новое заведение: {Name}", createDto.Name);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Errors(); // Использование метода расширения Errors()
                _logger.LogWarning("Модель не валидна: {Errors}", errors);
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(createDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Категория с ID {CategoryId} не найдена.", createDto.CategoryId);
                return BadRequest("Указанная категория не существует.");
            }

            var establishment = _mapper.Map<Establishment>(createDto);
            _logger.LogInformation("Сущность Establishment после маппинга: {Establishment}", establishment);

            if (createDto.TagIds != null && createDto.TagIds.Any())
            {
                var tags = await _context.Tags.Where(t => createDto.TagIds.Contains(t.Id)).ToListAsync();
                _logger.LogInformation("Найдено {Count} тегов для добавления.", tags.Count);

                if (tags.Count != createDto.TagIds.Count)
                {
                    var missingTagIds = createDto.TagIds.Except(tags.Select(t => t.Id)).ToList();
                    _logger.LogWarning("Теги с ID [{MissingTagIds}] не найдены.", string.Join(", ", missingTagIds));
                    return BadRequest($"Теги с ID [{string.Join(", ", missingTagIds)}] не существуют.");
                }

                foreach (var tag in tags)
                {
                    establishment.EstablishmentTags.Add(new EstablishmentTag { TagId = tag.Id });
                    _logger.LogInformation("Добавлен тег с ID {TagId} к EstablishmentTags.", tag.Id);
                }
            }

            try
            {
                _context.Establishments.Add(establishment);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Заведение с ID {Id} успешно создано.", establishment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении заведения: {Name}", createDto.Name);
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }

            var establishmentDto = _mapper.Map<EstablishmentDTO>(establishment);
            _logger.LogInformation("Сущность EstablishmentDTO после маппинга: {EstablishmentDto}", establishmentDto);

            return CreatedAtAction(nameof(GetEstablishment), new { id = establishment.Id }, establishmentDto);
        }

        // GET: api/Establishments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstablishmentDTO>> GetEstablishment(int id)
        {
            var establishment = await _context.Establishments
                .Include(e => e.Category)
                .Include(e => e.EstablishmentTags)
                    .ThenInclude(et => et.Tag)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (establishment == null)
            {
                _logger.LogWarning("Заведение с ID {Id} не найдено.", id);
                return NotFound();
            }

            var establishmentDto = _mapper.Map<EstablishmentDTO>(establishment);
            _logger.LogInformation("Возвращается EstablishmentDTO: {EstablishmentDto}", establishmentDto);

            return establishmentDto;
        }

        // PUT: api/Establishments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstablishment(int id, EstablishmentDTO establishmentDto)
        {
            if (id != establishmentDto.Id)
            {
                return BadRequest("ID в URL не совпадает с ID в теле запроса.");
            }

            var establishment = await _context.Establishments
                .Include(e => e.EstablishmentTags)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (establishment == null)
            {
                return NotFound();
            }

            // Обновление полей
            establishment.Name = establishmentDto.Name;
            establishment.CategoryId = establishmentDto.CategoryId;
            establishment.Address = establishmentDto.Address;
            establishment.Description = establishmentDto.Description;

            // Обновление тегов
            if (establishmentDto.TagIds != null)
            {
                // Удаление существующих связей
                _context.EstablishmentTags.RemoveRange(establishment.EstablishmentTags);

                // Добавление новых связей
                var tags = await _context.Tags.Where(t => establishmentDto.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    establishment.EstablishmentTags.Add(new EstablishmentTag { TagId = tag.Id });
                }

                // Проверка, все ли теги существуют
                var missingTagIds = establishmentDto.TagIds.Except(tags.Select(t => t.Id)).ToList();
                if (missingTagIds.Any())
                {
                    return BadRequest($"Теги с ID [{string.Join(", ", missingTagIds)}] не существуют.");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstablishmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Establishments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstablishment(int id)
        {
            var establishment = await _context.Establishments.FindAsync(id);
            if (establishment == null)
            {
                return NotFound();
            }

            _context.Establishments.Remove(establishment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstablishmentExists(int id)
        {
            return _context.Establishments.Any(e => e.Id == id);
        }
    }
}
