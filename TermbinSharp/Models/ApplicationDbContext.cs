using Microsoft.EntityFrameworkCore;

namespace TermbinSharp.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Data> Data { get; set; }
}