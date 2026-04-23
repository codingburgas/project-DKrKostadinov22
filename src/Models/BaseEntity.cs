using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PharmacyManager.Models
{
    // Абстрактен базов клас за наследяване и абстракция
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}