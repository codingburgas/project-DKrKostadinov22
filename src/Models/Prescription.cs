using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManager.Models
{
    public class Prescription : BaseEntity
    {
        [Required]
        public string DoctorName { get; set; } = null!;

        public int PatientId { get; set; } // Вече е int, за да съвпада с BaseEntity
        public virtual Patient Patient { get; set; } = null!;

        public decimal TotalValue { get; set; }

        public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();

        [Required]
        public Guid UserId { get; set; } // Свойството, което липсваше

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        public DateTime IssuedDate { get; set; }

        public Prescription()
        {
            PrescriptionItems = new List<PrescriptionItem>();
        }
        // ... other properties
    }
}
