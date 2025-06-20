using System.ComponentModel.DataAnnotations;

namespace GoodLuck.Models
{
    public class Anniversary
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string Title { get; set; } = string.Empty;
        [Required]

        public DateTime Date { get; set; }

        // Optional message to display when the event starts
        public string? Message { get; set; }

        // Icon to display for the event (emoji or character)
        [Required]
        public string Icon { get; set; } = "\uD83D\uDC95"; // default to 💕

        // Optional letter associated with this anniversary
        public string? LetterTitle { get; set; }
        public string? LetterContent { get; set; }
        public DateTime? LetterCreated { get; set; }
    }
}
