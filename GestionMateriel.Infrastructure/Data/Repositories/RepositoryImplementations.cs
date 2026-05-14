using GestionMateriel.Domain.Entities;
using GestionMateriel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionMateriel.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly GestionMaterielDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(GestionMaterielDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

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

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(GestionMaterielDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetWithStructuresAsync(int userId)
    {
        return await Context.Users
            .Include(u => u.UserStructures)
                .ThenInclude(us => us.Structure)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}

public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(GestionMaterielDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Item>> GetByStructureAsync(int structureId)
    {
        return await Context.Items
            .AsNoTracking()
            .Where(i => i.StructureId == structureId)
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

public class ItemCategoryRepository : Repository<ItemCategory>, IItemCategoryRepository
{
    public ItemCategoryRepository(GestionMaterielDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ItemCategory>> GetByStructureAsync(int structureId)
    {
        return await Context.ItemCategories
            .AsNoTracking()
            .Where(c => c.StructureId == structureId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}

public class ItemIssueRepository : Repository<ItemIssue>, IItemIssueRepository
{
    public ItemIssueRepository(GestionMaterielDbContext context) : base(context)
    {
    }

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

public class ItemIssueCommentRepository : Repository<ItemIssueComment>, IItemIssueCommentRepository
{
    public ItemIssueCommentRepository(GestionMaterielDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ItemIssueComment>> GetByIssueAsync(int itemIssueId)
    {
        return await Context.ItemIssueComments
            .AsNoTracking()
            .Where(c => c.ItemIssueId == itemIssueId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }
}

public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(GestionMaterielDbContext context) : base(context)
    {
    }

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

public class EventSubscriptionRepository : IEventSubscriptionRepository
{
    private readonly GestionMaterielDbContext _context;

    public EventSubscriptionRepository(GestionMaterielDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EventSubscription>> GetByEventAsync(int eventId)
    {
        return await _context.EventSubscriptions
            .AsNoTracking()
            .Where(es => es.EventId == eventId)
            .OrderBy(es => es.ItemId)
            .ToListAsync();
    }

    public async Task<EventSubscription?> GetAsync(int eventId, int itemId)
    {
        return await _context.EventSubscriptions
            .FirstOrDefaultAsync(es => es.EventId == eventId && es.ItemId == itemId);
    }

    public async Task<EventSubscription> AddAsync(EventSubscription subscription)
    {
        await _context.EventSubscriptions.AddAsync(subscription);
        return subscription;
    }

    public Task DeleteAsync(EventSubscription subscription)
    {
        _context.EventSubscriptions.Remove(subscription);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public class StructureRepository : Repository<Structure>, IStructureRepository
{
    public StructureRepository(GestionMaterielDbContext context) : base(context)
    {
    }

    public async Task<Structure?> GetWithMembersAsync(int id)
    {
        return await Context.Structures
            .Include(s => s.UserStructures)
                .ThenInclude(us => us.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}

public class UserStructureRepository : IUserStructureRepository
{
    private readonly GestionMaterielDbContext _context;

    public UserStructureRepository(GestionMaterielDbContext context)
    {
        _context = context;
    }

    public async Task<UserStructure?> GetAsync(int userId, int structureId)
    {
        return await _context.UserStructures
            .FirstOrDefaultAsync(us => us.UserId == userId && us.StructureId == structureId);
    }

    public async Task<IEnumerable<UserStructure>> GetByUserAsync(int userId)
    {
        return await _context.UserStructures
            .AsNoTracking()
            .Where(us => us.UserId == userId)
            .Include(us => us.Structure)
            .ToListAsync();
    }

    public async Task AddAsync(UserStructure userStructure)
    {
        await _context.UserStructures.AddAsync(userStructure);
    }

    public Task DeleteAsync(UserStructure userStructure)
    {
        _context.UserStructures.Remove(userStructure);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(GestionMaterielDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await Context.RefreshTokens
            .Include(rt => rt.User)
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
