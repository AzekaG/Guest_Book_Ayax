using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Guest_Book_Ayax.Models
{
    public class UsersContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
