using System.ComponentModel.DataAnnotations;

namespace EstablishmentAPI.DTOs
{
    public class TagDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
