using System.ComponentModel.DataAnnotations;

namespace PharmacyManager.Models
{
    public class Patient : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string EGN { get; set; } = null!;

        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
