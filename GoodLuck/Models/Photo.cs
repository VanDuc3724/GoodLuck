using System;

namespace GoodLuck.Models
{
    public class Photo
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Uploaded { get; set; }
    }
}
