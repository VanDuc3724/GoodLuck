using System;
using System.ComponentModel.DataAnnotations;

namespace GoodLuck.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Uploaded { get; set; }
    }
}
