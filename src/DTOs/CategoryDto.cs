namespace PharmacyManager.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int MedicamentsCount { get; set; } // Показва колко лекарства има в тази категория
    }
}
