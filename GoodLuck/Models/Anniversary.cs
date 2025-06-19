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
    }
}
