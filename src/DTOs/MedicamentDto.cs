using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManager.DTOs
{
    public class MedicamentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please select a manufacturer")]
        public int ManufacturerId { get; set; }

        // ТРЯБВА ДА ИМА '?', защото не идва от формата
        public string? ManufacturerName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public List<int> CategoryIds { get; set; } = new();

        // ТРЯБВА ДА ИМА '?', иначе валидацията очаква списък с имена
        public List<string>? CategoryNames { get; set; } = new();
    }
}