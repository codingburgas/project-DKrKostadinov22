using System.ComponentModel.DataAnnotations;

namespace PharmacyManager.DTOs
{
    public class PrescriptionItemDto
    {
        [Required(ErrorMessage = "Please select a medicine.")]
        public int MedicamentId { get; set; }

        public string MedicamentName { get; set; } = null!;

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
