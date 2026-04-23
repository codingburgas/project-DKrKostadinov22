namespace PharmacyManager.DTOs
{
    public class DashboardStatisticsDto
    {
        public int TotalMedicaments { get; set; }
        public int LowStockCount { get; set; } // Брой лекарства под критичния праг
        public decimal AveragePrescriptionValue { get; set; }
        public List<KeyValuePair<string, int>> TopSellingMedicaments { get; set; } = new();
    }
}
