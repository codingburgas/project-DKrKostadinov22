namespace PharmacyManager.DTOs
{
    public class MedicamentListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
