namespace SongNewApi.Models
{
    public class Base
    {
        public int BaseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; } 
        public string? ModifiedBy { get; set; }
        public bool isRowDeleted { get; set; }

    }
}
