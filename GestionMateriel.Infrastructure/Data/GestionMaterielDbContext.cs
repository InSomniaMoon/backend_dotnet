using GestionMateriel.Domain.Entities;
using GestionMateriel.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Data;

public class GestionMaterielDbContext(DbContextOptions<GestionMaterielDbContext> options) : DbContext(options)
{
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


    }
}

