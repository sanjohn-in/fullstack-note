using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Notes;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]   // All notes endpoints require a valid JWT
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INotesService notesService, ILogger<NotesController> logger)
        {
            _notesService = notesService;
            _logger = logger;
        }

        // Helper to get the current user's ID from JWT claims
        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET api/notes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notes = await _notesService.GetAllAsync(GetUserId());
            _logger.LogInformation("Retrieved all notes for user ID: {UserId}", GetUserId());
            return Ok(notes);
        }

        // GET api/notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var note = await _notesService.GetByIdAsync(id, GetUserId());
                return Ok(note);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST api/notes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteDto dto)
        {
            var note = await _notesService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        // PUT api/notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteDto dto)
        {
            try
            {
                var note = await _notesService.UpdateAsync(id, dto, GetUserId());
                return Ok(note);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _notesService.DeleteAsync(id, GetUserId());
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}