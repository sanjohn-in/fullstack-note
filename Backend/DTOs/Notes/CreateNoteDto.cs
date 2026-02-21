using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Notes
{
    public class CreateNoteDto
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }
    }
}