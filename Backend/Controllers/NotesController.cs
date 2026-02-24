using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Notes;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        // GET api/notes/{userId}?page=1&pageSize=10
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(
            int userId,
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _notesService.GetAllAsync(userId, search, page, pageSize);
            return Ok(result);
        }

        // GET api/notes/{userId}/{id}
        [HttpGet("{userId}/{id}")]
        public async Task<IActionResult> GetById(int userId, int id)
        {
            var note = await _notesService.GetByIdAsync(id, userId);
            return Ok(note);
        }

        // POST api/notes/{userId}
        [HttpPost("{userId}")]
        public async Task<IActionResult> Create(int userId, [FromBody] CreateNoteDto dto)
        {
            var note = await _notesService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById),
                new { userId = userId, id = note.Id }, note);
        }

        // PUT api/notes/{userId}/{id}
        [HttpPut("{userId}/{id}")]
        public async Task<IActionResult> Update(
            int userId,
            int id,
            [FromBody] UpdateNoteDto dto)
        {
            var note = await _notesService.UpdateAsync(id, dto, userId);
            return Ok(note);
        }

        // DELETE api/notes/{userId}/{id}
        [HttpDelete("{userId}/{id}")]
        public async Task<IActionResult> Delete(int userId, int id)
        {
            await _notesService.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}