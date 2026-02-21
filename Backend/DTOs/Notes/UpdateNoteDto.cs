using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Notes
{
    public class UpdateNoteDto
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }
    }
}