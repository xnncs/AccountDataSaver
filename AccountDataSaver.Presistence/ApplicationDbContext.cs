using AccountDataSaver.Presistence.Configurations;
using AccountDataSaver.Presistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace AccountDataSaver.Presistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserAccountEntity> UserAccounts { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserAccountEntityConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}