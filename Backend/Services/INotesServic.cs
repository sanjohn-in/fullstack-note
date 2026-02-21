using Backend.DTOs.Notes;

namespace Backend.Services
{
    public interface INotesService
    {
        Task<IEnumerable<NoteResponseDto>> GetAllAsync(int userId);
        Task<NoteResponseDto> GetByIdAsync(int noteId, int userId);
        Task<NoteResponseDto> CreateAsync(CreateNoteDto dto, int userId);
        Task<NoteResponseDto> UpdateAsync(int noteId, UpdateNoteDto dto, int userId);
        Task DeleteAsync(int noteId, int userId);
    }
}