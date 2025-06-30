using Calabonga.OopExercises.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Calabonga.OopExercises.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Document> Documents { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<DocumentState> DocumentStates { get; set; }

    public DbSet<Participant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}


public class ApplicationDbContextContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=OopExercises;User ID=postgres;Password=8jkGh47hnDw89Haq8LN2;TrustServerCertificate=False;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
