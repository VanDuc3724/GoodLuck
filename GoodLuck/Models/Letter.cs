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
    }
}
