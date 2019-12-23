
using Microsoft.EntityFrameworkCore;
namespace BankAccount.Models
{
    public class HomeContext : DbContext 
    {
        public HomeContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users {get; set;}
        public DbSet<TransActions> TransAction {get; set;}
    }
}