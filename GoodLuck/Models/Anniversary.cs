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

        [Required]
        public string SubTitle { get; set; } = string.Empty;

        [Required]
        public string Icon { get; set; } = "\uD83D\uDC95"; // default to 💕

        [Required]
        public string LetterContent { get; set; } = string.Empty;

    }
}
