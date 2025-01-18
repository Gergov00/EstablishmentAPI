using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstablishmentAPI.DTOs
{
    public class CreateEstablishmentDTO
    {
        [Required(ErrorMessage = "Имя заведения обязательно.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Категория обязательна.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Адрес обязателен.")]
        public string Address { get; set; }

        public string Description { get; set; }

        public List<int> TagIds { get; set; }
    }
}
