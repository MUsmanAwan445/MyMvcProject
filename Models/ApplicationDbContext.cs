using Microsoft.EntityFrameworkCore;

namespace MyMvcProject.Models
{
    // Ye hamari main database context class hai jo Entity Framework ko handle karti hai
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Ye hamara main table hai jismein form ka sara data save hoga
        // Isay direct WorkServiceFormViewModel se map kiya gaya hai taake extra models ki zaroorat na pare aur migrations ka masla na ho
        public DbSet<WorkServiceFormViewModel> WorkServiceForms { get; set; }
        
        // Ye designations ka table hai jo dropdowns ke liye use hoga
        public DbSet<Designation> Designations { get; set; }
    }

    // ==========================================
    // Dropdown ke liye Naya Table (Designations)
    // ==========================================
    public class Designation
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}