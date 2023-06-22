using Microsoft.EntityFrameworkCore;

using WebApiDemo.Models.Registration;

namespace WebApiDemo.Data
{
    public class RegistrationApiDbContext: DbContext
    {
        public RegistrationApiDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Registration> Registrations { get; set; }

       
    }
}




