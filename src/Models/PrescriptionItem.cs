namespace PharmacyManager.Models
{
    public class PrescriptionItem
    {
        // Тук също са int
        public int PrescriptionId { get; set; }
        public virtual Prescription Prescription { get; set; } = null!;

        public int MedicamentId { get; set; }
        public virtual Medicament Medicament { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
