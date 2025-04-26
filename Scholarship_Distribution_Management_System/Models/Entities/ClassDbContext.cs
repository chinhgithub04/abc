using Microsoft.EntityFrameworkCore;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class ClassDbContext : DbContext
    {
        public ClassDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
