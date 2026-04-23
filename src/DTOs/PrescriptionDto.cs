using System.ComponentModel.DataAnnotations;

namespace PharmacyManager.DTOs
{
    public class PrescriptionDto
    {
        // ПРОМЕНИ ТОВА: от public Guid Id на public int Id
        public int Id { get; set; }

        public string DoctorName { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public string PatientEgn { get; set; } = null!;
        public DateTime IssuedDate { get; set; }
        public decimal TotalValue { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();

        public int MedicamentId { get; set; }
        public int Quantity { get; set; }
    }
}
