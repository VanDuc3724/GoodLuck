using System.ComponentModel.DataAnnotations;

namespace GoodLuck.Models
{
    public class Letter
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string Title { get; set; } = string.Empty;
        [Required]

        public string Content { get; set; } = string.Empty;
        [Required]

        public DateTime Created { get; set; }

        // Link to an anniversary so the letter can be displayed
        // when the event occurs
        [Required]
        public int AnniversaryId { get; set; }
        public Anniversary? Anniversary { get; set; }
    }
}
