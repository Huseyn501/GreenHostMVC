using System.ComponentModel.DataAnnotations;

namespace GreenHost.Models
{
    public class TeamMembers
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        public string Position { get; set; }
        public string? Imageurl { get; set; }
    }
}
