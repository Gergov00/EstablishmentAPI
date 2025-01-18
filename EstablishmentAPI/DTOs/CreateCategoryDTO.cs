using System.ComponentModel.DataAnnotations;

namespace EstablishmentAPI.DTOs
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Имя категории обязательно.")]
        [StringLength(100, ErrorMessage = "Имя категории не может превышать 100 символов.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Описание категории не может превышать 500 символов.")]
        public string Description { get; set; }
    }
}
