using System.Collections.Generic;

namespace EstablishmentAPI.DTOs
{
    public class EstablishmentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public List<int> TagIds { get; set; }
    }
}
