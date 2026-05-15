using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionMateriel.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(u => u.Password)
            .IsRequired();

        builder.Property(u => u.Phone)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasMany(u => u.UserStructures)
            .WithOne(us => us.User)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ReportedIssues)
            .WithOne(ii => ii.ReportedBy)
            .HasForeignKey(ii => ii.ReportedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.CreatedEvents)
            .WithOne(e => e.CreatedBy)
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.FeatureClicks)
            .WithOne(fc => fc.User)
            .HasForeignKey(fc => fc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ValueConverter for RoleEnum
        builder.Property(u => u.Role)
            .HasConversion(
                v => v.Value,
                v => RoleEnum.FromString(v)
            )
            .HasMaxLength(20)
            .IsUnicode(false);
    }
}

public class StructureConfiguration : IEntityTypeConfiguration<Structure>
{
    public void Configure(EntityTypeBuilder<Structure> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.CodeStructure)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(s => s.NomStructure)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.ParentCode)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(s => s.Color)
            .HasMaxLength(7)
            .IsUnicode(false);

        builder.Property(s => s.ImagePath)
            .HasMaxLength(500);
    }
}

public class UserStructureConfiguration : IEntityTypeConfiguration<UserStructure>
{
    public void Configure(EntityTypeBuilder<UserStructure> builder)
    {
        builder.HasKey(us => new { us.UserId, us.StructureId });

        builder.HasOne(us => us.User)
            .WithMany(u => u.UserStructures)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(us => us.Structure)
            .WithMany(s => s.UserStructures)
            .HasForeignKey(us => us.StructureId)
            .OnDelete(DeleteBehavior.Cascade);

        // ValueConverter for RoleEnum
        builder.Property(us => us.Role)
            .HasConversion(
                v => v.Value,
                v => RoleEnum.FromString(v)
            )
            .HasMaxLength(20)
            .IsUnicode(false);
    }
}

public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
{
    public void Configure(EntityTypeBuilder<ItemCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(c => c.Structure)
            .WithMany(s => s.ItemCategories)
            .HasForeignKey(c => c.StructureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.Description)
            .HasMaxLength(1000);

        builder.Property(i => i.ImagePath)
            .HasMaxLength(500);

        builder.Property(i => i.CodeStructure)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Structure)
            .WithMany(s => s.Items)
            .HasForeignKey(i => i.StructureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ItemIssueConfiguration : IEntityTypeConfiguration<ItemIssue>
{
    public void Configure(EntityTypeBuilder<ItemIssue> builder)
    {
        builder.HasKey(ii => ii.Id);

        builder.Property(ii => ii.Value)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(ii => ii.Item)
            .WithMany(i => i.Issues)
            .HasForeignKey(ii => ii.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ii => ii.ReportedBy)
            .WithMany(u => u.ReportedIssues)
            .HasForeignKey(ii => ii.ReportedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ItemIssueCommentConfiguration : IEntityTypeConfiguration<ItemIssueComment>
{
    public void Configure(EntityTypeBuilder<ItemIssueComment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Comment)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(c => c.ItemIssue)
            .WithMany(ii => ii.Comments)
            .HasForeignKey(c => c.ItemIssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Comment)
            .HasMaxLength(1000);

        builder.HasOne(e => e.Structure)
            .WithMany(s => s.Events)
            .HasForeignKey(e => e.StructureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.CreatedBy)
            .WithMany(u => u.CreatedEvents)
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class EventSubscriptionConfiguration : IEntityTypeConfiguration<EventSubscription>
{
    public void Configure(EntityTypeBuilder<EventSubscription> builder)
    {
        builder.HasKey(es => new { es.EventId, es.ItemId });

        builder.HasOne(es => es.Event)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(es => es.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(es => es.Item)
            .WithMany(i => i.EventSubscriptions)
            .HasForeignKey(es => es.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(f => f.Slug)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.HasIndex(f => f.Slug)
            .IsUnique();
    }
}

public class FeatureClickConfiguration : IEntityTypeConfiguration<FeatureClick>
{
    public void Configure(EntityTypeBuilder<FeatureClick> builder)
    {
        builder.HasKey(fc => fc.Id);

        builder.HasOne(fc => fc.Feature)
            .WithMany(f => f.Clicks)
            .HasForeignKey(fc => fc.FeatureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fc => fc.User)
            .WithMany(u => u.FeatureClicks)
            .HasForeignKey(fc => fc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
            .IsRequired();

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
