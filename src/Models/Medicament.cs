using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManager.Models
{
    public class Medicament : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Range(0, 10000)]
        public int StockQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime ExpiryDate { get; set; }

        // One-to-Many relationship
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; } = null!;

        // Many-to-Many relationship join table
        public virtual ICollection<MedicamentCategory> MedicamentCategories { get; set; } = new List<MedicamentCategory>();
    }
}
