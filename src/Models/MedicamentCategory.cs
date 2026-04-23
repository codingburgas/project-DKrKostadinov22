namespace PharmacyManager.Models
{
    public class MedicamentCategory
    {
        public int MedicamentId { get; set; }
        public virtual Medicament Medicament { get; set; } = null!;

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
}
