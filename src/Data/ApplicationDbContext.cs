using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmacyManager.Models;

namespace PharmacyManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Medicament> Medicaments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<MedicamentCategory> MedicamentCategories { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; } = null!;
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ЗАДЪЛЖИТЕЛНО: Базова конфигурация за Identity (User, Roles, Claims)
            base.OnModelCreating(builder);

            // --- 1. MEDICAMENT & CATEGORY (Many-to-Many) ---
            builder.Entity<MedicamentCategory>()
                .HasKey(mc => new { mc.MedicamentId, mc.CategoryId });

            builder.Entity<MedicamentCategory>()
                .HasOne(mc => mc.Medicament)
                .WithMany(m => m.MedicamentCategories)
                .HasForeignKey(mc => mc.MedicamentId);

            builder.Entity<MedicamentCategory>()
                .HasOne(mc => mc.Category)
                .WithMany(c => c.MedicamentCategories)
                .HasForeignKey(mc => mc.CategoryId);


            // --- 2. PRESCRIPTION & MEDICAMENT (Many-to-Many via PrescriptionItem) ---
            builder.Entity<PrescriptionItem>()
                .HasKey(pi => new { pi.PrescriptionId, pi.MedicamentId });

            builder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Prescription)
                .WithMany(p => p.PrescriptionItems)
                .HasForeignKey(pi => pi.PrescriptionId);

            builder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Medicament)
                .WithMany() // Лекарството не се нуждае от списък с всички рецепти, в които е участвало
                .HasForeignKey(pi => pi.MedicamentId);


            // --- 3. MANUFACTURER & MEDICAMENT (One-to-Many) ---
            builder.Entity<Medicament>()
                .HasOne(m => m.Manufacturer)
                .WithMany(man => man.Medicaments)
                .HasForeignKey(m => m.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict); // Предпазва от изтриване на производител, ако има лекарства


            // --- 4. PRECISION CONFIGURATION (Decimal types) ---
            builder.Entity<Medicament>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);

            builder.Entity<Prescription>()
                .Property(p => p.TotalValue)
                .HasPrecision(18, 2);

            builder.Entity<PrescriptionItem>()
                .Property(pi => pi.UnitPrice)
                .HasPrecision(18, 2);


            // --- 5. INDEXES & CONSTRAINTS ---
            builder.Entity<Patient>()
                .HasIndex(p => p.EGN)
                .IsUnique();


            // --- 6. DATA SEEDING (Начални данни) ---
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Antibiotics" },
                new Category { Id = 2, Name = "Analgesics" },
                new Category { Id = 3, Name = "Supplements" },
                new Category { Id = 4, Name = "Vitamins" }
            );

            builder.Entity<Prescription>()
        .HasOne(p => p.User)
        .WithMany() // Един потребител може да има много рецепти
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.Restrict); // Предотвратява изтриване на потребител, ако има рецепти
        }
    }
}