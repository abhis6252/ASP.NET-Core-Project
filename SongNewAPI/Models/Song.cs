using System.ComponentModel.DataAnnotations;

namespace SongNewApi.Models
{
    public class Song : Base
    {
        public int Id { get; set; }
        public string? ArtistName { get; set; }
        public string? FilePath { get; set; }

        public int ArtistId { get; set; }
        public int CategoryId { get; set; }

        public Artist Artist { get; set; }
        public Category Category { get; set; }
    }
}
