namespace PharmacyManager.DTOs
{
    public class DashboardDto
    {
        public int TotalMedicaments { get; set; }
        public int LowStockCount { get; set; } // Списък с лекарства с критично ниска наличност (Low Stock Alert)
        public decimal AveragePrescriptionValue { get; set; } // Средна стойност на обработена рецепта
        public Dictionary<string, int> TopSellingMedicaments { get; set; } = new(); // Най-продавани медикаменти
    }
}
