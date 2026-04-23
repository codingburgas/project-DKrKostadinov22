using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManager.Models
{
    public class Manufacturer : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }

        // Релация: Един производител има много лекарства (One-to-Many)
        public virtual ICollection<Medicament> Medicaments { get; set; } = new List<Medicament>();
    }
}