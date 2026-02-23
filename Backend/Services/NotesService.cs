using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs.Notes;
using Backend.Models;

namespace Backend.Services
{
    public class NotesService : INotesService
    {
        private readonly AppDbContext _db;

        public NotesService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<object> GetAllAsync(int userId, string? search, int page, int pageSize)
        {
            var query = _db.Notes
                .Where(n => n.UserId == userId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(n => n.Title.Contains(search) || n.Content.Contains(search));

            var total = await query.CountAsync();

            var notes = await query
                .OrderByDescending(n => n.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => ToDto(n))
                .ToListAsync();

            return new
            {
                data = notes,
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)total / pageSize)
            };
        }
        public async Task<NoteResponseDto> GetByIdAsync(int noteId, int userId)
        {
            var note = await _db.Notes
                .FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId)
                ?? throw new Exception("Note not found.");

            return ToDto(note);
        }

        public async Task<NoteResponseDto> CreateAsync(CreateNoteDto dto, int userId)
        {
            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Notes.Add(note);
            await _db.SaveChangesAsync();

            return ToDto(note);
        }

        public async Task<NoteResponseDto> UpdateAsync(int noteId, UpdateNoteDto dto, int userId)
        {
            var note = await _db.Notes
                .FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId)
                ?? throw new Exception("Note not found.");

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.UtcNow;   // Auto-update timestamp

            await _db.SaveChangesAsync();

            return ToDto(note);
        }

        public async Task DeleteAsync(int noteId, int userId)
        {
            var note = await _db.Notes
                .FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId)
                ?? throw new Exception("Note not found.");

            _db.Notes.Remove(note);
            await _db.SaveChangesAsync();
        }

        private static NoteResponseDto ToDto(Note note) => new()
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }
}