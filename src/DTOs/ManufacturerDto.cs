using System.ComponentModel.DataAnnotations;

namespace PharmacyManager.DTOs
{
    public class ManufacturerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Manufacturer name is required.")]
        public string Name { get; set; } = null!;

        public string? Address { get; set; }
    }
}
