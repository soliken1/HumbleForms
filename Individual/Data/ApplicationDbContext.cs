using Individual.Models;
using Microsoft.EntityFrameworkCore;

namespace Individual.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>()
                .HasKey(s => new { s.SUBJCODE, s.SUBJCOURSECODE, s.SUBJCATEGORY, s.SUBJCODEPREQ });
        }
    }
}
