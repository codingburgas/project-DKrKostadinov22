using System.ComponentModel.DataAnnotations;

namespace PharmacyManager.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        // Many-to-Many navigation
        public virtual ICollection<MedicamentCategory> MedicamentCategories { get; set; } = new List<MedicamentCategory>();
    }
}
