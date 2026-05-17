using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Data.Repositories;

public class Repository<T>(GestionMaterielDbContext context) : IRepository<T> where T : class
{
    protected readonly GestionMaterielDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public virtual Task<T> UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return Task.FromResult(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
        {
            return;
        }

        DbSet.Remove(entity);
    }

    public virtual async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}

public class UserRepository(GestionMaterielDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.AsNoTracking().AsSplitQuery()
                .Include(u => u.UserStructures)
                .ThenInclude(us => us.Structure)
                .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetWithStructuresAsync(int userId)
    {
        return await Context.Users
            .Include(u => u.UserStructures)
                .ThenInclude(us => us.Structure)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public Task<bool> IsEmailTakenAsync(string email)
    {
        return Context.Users.AnyAsync(u => u.Email == email);
    }
}

public class ItemRepository(GestionMaterielDbContext context) : Repository<Item>(context), IItemRepository
{
    public async Task<IEnumerable<Item>> GetByStructureAsync()
    {
        return await Context.Items

            .AsNoTracking()
            .Include(i => i.Category)
            .Include(i => i.Issues.Where(ii => ii.Status == Domain.Enums.IssueStatusEnum.Open))
            // .Select(i => new()
            // {
            //     Id = i.Id,
            //     Name = i.Name,
            //     Description = i.Description,
            //     CategoryId = i.CategoryId,
            //     StructureId = i.StructureId,
            //     Usable = i.Usable,
            //     Stock = i.Stock,
            //     DateOfBuy = i.DateOfBuy,
            //     Image = i.Image,
            //     CodeStructure = i.CodeStructure,
            //     Category = i.Category != null ? new ItemCategory
            //     {
            //         Id = i.Category.Id,
            //         Name = i.Category.Name,
            //         StructureId = i.Category.StructureId,
            //         Identified = i.Category.Identified
            //     } : null,

            //     // openIssuesCount = i.Issues.Count
            // })
            .ToListAsync();
    }

    public async Task<IEnumerable<Item>> GetAvailableItemsAsync(int structureId)
    {
        return await Context.Items
            .AsNoTracking()
            .Where(i => i.StructureId == structureId && i.Usable && i.Stock > 0)
            .ToListAsync();
    }
}

public class ItemCategoryRepository(GestionMaterielDbContext context) : Repository<ItemCategory>(context), IItemCategoryRepository
{
    public async Task<IEnumerable<ItemCategory>> GetByStructureAsync()
    {
        return await Context.ItemCategories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}

public class ItemIssueRepository(GestionMaterielDbContext context) : Repository<ItemIssue>(context), IItemIssueRepository
{
    public async Task<IEnumerable<ItemIssue>> GetOpenIssuesAsync()
    {
        return await Context.ItemIssues
            .AsNoTracking()
            .Where(ii => ii.Status == Domain.Enums.IssueStatusEnum.Open)
            .OrderByDescending(ii => ii.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemIssue>> GetByItemAsync(int itemId)
    {
        return await Context.ItemIssues
            .AsNoTracking()
            .Where(ii => ii.ItemId == itemId)
            .OrderByDescending(ii => ii.CreatedAt)
            .ToListAsync();
    }
}

public class ItemIssueCommentRepository(GestionMaterielDbContext context) : Repository<ItemIssueComment>(context), IItemIssueCommentRepository
{
    public async Task<IEnumerable<ItemIssueComment>> GetByIssueAsync(int itemIssueId)
    {
        return await Context.ItemIssueComments
            .AsNoTracking()
            .Where(c => c.ItemIssueId == itemIssueId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }
}

public class EventRepository(GestionMaterielDbContext context) : Repository<Event>(context), IEventRepository
{
    public async Task<IEnumerable<Event>> GetEventsByStructureAsync(int structureId)
    {
        return await Context.Events
            .AsNoTracking()
            .Where(e => e.StructureId == structureId)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetActualEventsAsync()
    {
        var now = DateTime.UtcNow;
        return await Context.Events
            .AsNoTracking()
            .Where(e => e.EndDate >= now)
            .OrderBy(e => e.StartDate)
            .ToListAsync();
    }
}

public class EventSubscriptionRepository(GestionMaterielDbContext context) : IEventSubscriptionRepository
{
    public async Task<IEnumerable<EventSubscription>> GetByEventAsync(int eventId)
    {
        return await context.EventSubscriptions
            .AsNoTracking()
            .Where(es => es.EventId == eventId)
            .OrderBy(es => es.ItemId)
            .ToListAsync();
    }

    public async Task<EventSubscription?> GetAsync(int eventId, int itemId)
    {
        return await context.EventSubscriptions
            .FirstOrDefaultAsync(es => es.EventId == eventId && es.ItemId == itemId);
    }

    public async Task<EventSubscription> AddAsync(EventSubscription subscription)
    {
        await context.EventSubscriptions.AddAsync(subscription);
        return subscription;
    }

    public Task DeleteAsync(EventSubscription subscription)
    {
        context.EventSubscriptions.Remove(subscription);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

public class StructureRepository(GestionMaterielDbContext context) : Repository<Structure>(context), IStructureRepository
{
    public async Task<Structure?> GetWithMembersAsync(int id)
    {
        return await Context.Structures
            .Include(s => s.UserStructures)
                .ThenInclude(us => us.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}

public class UserStructureRepository(GestionMaterielDbContext context) : IUserStructureRepository
{
    public async Task<UserStructure?> GetAsync(int userId, int structureId)
    {
        return await context.UserStructures
            .FirstOrDefaultAsync(us => us.UserId == userId && us.StructureId == structureId);
    }

    public async Task<IEnumerable<UserStructure>> GetByUserAsync(int userId)
    {
        return await context.UserStructures
            .AsNoTracking()
            .Where(us => us.UserId == userId)
            .Include(us => us.Structure)
            .ToListAsync();
    }

    public async Task AddAsync(UserStructure userStructure)
    {
        await context.UserStructures.AddAsync(userStructure);
    }

    public Task DeleteAsync(UserStructure userStructure)
    {
        context.UserStructures.Remove(userStructure);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

public class RefreshTokenRepository(GestionMaterielDbContext context) : Repository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await Context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.UserStructures)
            .ThenInclude(us => us.Structure)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task DeleteExpiredTokensAsync()
    {
        var now = DateTime.UtcNow;
        var expiredTokens = await Context.RefreshTokens
            .Where(rt => rt.ExpiresAt < now)
            .ToListAsync();

        if (expiredTokens.Count == 0)
        {
            return;
        }

        Context.RefreshTokens.RemoveRange(expiredTokens);
        await Context.SaveChangesAsync();
    }
}
