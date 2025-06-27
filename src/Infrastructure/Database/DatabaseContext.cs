namespace Infrastructure.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) 
    : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in this.ChangeTracker.Entries<IEntity>())
        {
            switch (entry)
            {
                case { State: EntityState.Added }:
                    entry.Entity.CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    entry.Entity.ModifiedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    entry.Entity.IsActive = true;
                    break;
                
                case { State: EntityState.Modified }:
                    entry.Entity.ModifiedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    break;
                
                case { State: EntityState.Deleted }:
                    entry.State = EntityState.Modified;
                    entry.Entity.ModifiedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    entry.Entity.IsActive = false;
                    break;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.Email).IsRequired();
        });

        modelBuilder.Entity<Payroll>(entity =>
        {            
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}