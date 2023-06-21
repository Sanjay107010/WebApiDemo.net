using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;

namespace WebApiDemo.Data
{
    public class ContactApiDbContext : DbContext
    {
        public ContactApiDbContext(DbContextOptions options) : base(options)
        {
        }
       public DbSet <Contact> Contacts { get; set; }
    }
}
