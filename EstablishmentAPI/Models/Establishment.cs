// Models/Establishment.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstablishmentAPI.Models
{
    public class Establishment
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Address { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public ICollection<EstablishmentTag> EstablishmentTags { get; set; } = new List<EstablishmentTag>();
    }
}
