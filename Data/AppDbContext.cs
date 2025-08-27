using Microsoft.EntityFrameworkCore;
using MessageApi.Models;
using HelloApi.Models;

namespace MessageApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Product> Products => Set<Product>();
    
}
