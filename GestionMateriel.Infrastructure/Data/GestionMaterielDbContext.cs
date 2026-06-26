using GestionMateriel.Application.Services;
using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Infrastructure.Data.Configurations;
using GestionMateriel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GestionMateriel.Infrastructure.Data;

public class GestionMaterielDbContext(
    DbContextOptions<GestionMaterielDbContext> options,
    ITenantProvider tenantProvider,
    IOptions<JwtSettings> jwtSettings
) : DbContext(options)
{
    private readonly ITenantProvider _tenantProvider = tenantProvider;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private bool HasTenantMask => !string.IsNullOrWhiteSpace(_tenantProvider.StructureMask);
    private string TenantMask => _tenantProvider.StructureMask ?? string.Empty;
    private string? TenantStructureCode => _tenantProvider.StructureCode;

    public DbSet<User> Users => Set<User>();
    public DbSet<Structure> Structures => Set<Structure>();
    public DbSet<UserStructure> UserStructures => Set<UserStructure>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();
    public DbSet<ItemIssue> ItemIssues => Set<ItemIssue>();
    public DbSet<ItemIssueComment> ItemIssueComments => Set<ItemIssueComment>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventSubscription> EventSubscriptions => Set<EventSubscription>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<FeatureClick> FeatureClicks => Set<FeatureClick>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new StructureConfiguration());
        modelBuilder.ApplyConfiguration(new UserStructureConfiguration());
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new ItemCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ItemIssueConfiguration());
        modelBuilder.ApplyConfiguration(new ItemIssueCommentConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new EventSubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new FeatureConfiguration());
        modelBuilder.ApplyConfiguration(new FeatureClickConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new PasswordResetTokenConfiguration());

        ApplyTenantQueryFilters(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        NormalizeTrackedDateTimesToUtc();
        ApplyTenantScopeOnAddedEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        NormalizeTrackedDateTimesToUtc();
        ApplyTenantScopeOnAddedEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void NormalizeTrackedDateTimesToUtc()
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.ClrType == typeof(DateTime) && property.CurrentValue is DateTime dt)
                {
                    property.CurrentValue = dt.Kind switch
                    {
                        DateTimeKind.Utc => dt,
                        DateTimeKind.Local => dt.ToUniversalTime(),
                        _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
                    };
                }

                if (property.Metadata.ClrType == typeof(DateTime?) && property.CurrentValue is DateTime nullableDt)
                {
                    property.CurrentValue = nullableDt.Kind switch
                    {
                        DateTimeKind.Utc => nullableDt,
                        DateTimeKind.Local => nullableDt.ToUniversalTime(),
                        _ => DateTime.SpecifyKind(nullableDt, DateTimeKind.Utc)
                    };
                }
            }
        }
    }

    protected virtual void ApplyTenantQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Structure>()
            .HasQueryFilter(s => !HasTenantMask || s.CodeStructure.StartsWith(TenantMask));

        modelBuilder.Entity<UserStructure>()
            .HasQueryFilter(us => !HasTenantMask || us.Structure.CodeStructure.StartsWith(TenantMask));

        modelBuilder.Entity<ItemCategory>()
            .HasQueryFilter(c => !HasTenantMask || c.Structure.CodeStructure.StartsWith(TenantMask));

        modelBuilder.Entity<Item>()
            .HasQueryFilter(i => !HasTenantMask || (i.CodeStructure != null && i.CodeStructure.StartsWith(TenantMask)));

        modelBuilder.Entity<ItemIssue>()
            .HasQueryFilter(ii =>
                !HasTenantMask || (ii.Item.CodeStructure != null && ii.Item.CodeStructure.StartsWith(TenantMask)));

        modelBuilder.Entity<ItemIssueComment>()
            .HasQueryFilter(c => !HasTenantMask || (c.ItemIssue.Item.CodeStructure != null &&
                                                    c.ItemIssue.Item.CodeStructure.StartsWith(TenantMask)));

        modelBuilder.Entity<Event>()
            .HasQueryFilter(e => !HasTenantMask || e.Structure.CodeStructure.StartsWith(TenantMask));

        modelBuilder.Entity<EventSubscription>()
            .HasQueryFilter(es => !HasTenantMask || es.Event.Structure.CodeStructure.StartsWith(TenantMask));
    }

    private void ApplyTenantScopeOnAddedEntities()
    {
        if (!_tenantProvider.IsResolved)
        {
            return;
        }

        foreach (var entry in ChangeTracker.Entries<IStructureCodeScopedEntity>()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            if (string.IsNullOrWhiteSpace(entry.Entity.CodeStructure) &&
                !string.IsNullOrWhiteSpace(TenantStructureCode))
            {
                entry.Entity.CodeStructure = TenantStructureCode;
            }

            if (HasTenantMask
                && !string.IsNullOrWhiteSpace(entry.Entity.CodeStructure)
                && !entry.Entity.CodeStructure.StartsWith(TenantMask, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException("Le code structure n'appartient pas au tenant courant.");
            }
        }
    }
}

