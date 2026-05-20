using GestionMateriel.Domain.Entities;

namespace GestionMateriel.Domain.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetWithStructuresAsync(int userId);
    Task<bool> IsEmailTakenAsync(string email);
}

public interface IStructureRepository : IRepository<Structure>
{
    Task<Structure?> GetWithMembersAsync(int id);
}

public interface IItemRepository : IRepository<Item>
{
    Task<(IEnumerable<Item>, int)> GetByStructureAsync(int Page, int Size, string? Q, string? OrderBy, string? OrderDir, CancellationToken cancellationToken);
    Task<IEnumerable<Item>> GetAvailableItemsAsync(int structureId);
}

public interface IItemCategoryRepository : IRepository<ItemCategory>
{
    Task<IEnumerable<ItemCategory>> GetByStructureAsync();
}

public interface IItemIssueRepository : IRepository<ItemIssue>
{
    Task<IEnumerable<ItemIssue>> GetOpenIssuesAsync();
    Task<IEnumerable<ItemIssue>> GetByItemAsync(int itemId);
}

public interface IItemIssueCommentRepository : IRepository<ItemIssueComment>
{
    Task<IEnumerable<ItemIssueComment>> GetByIssueAsync(int itemIssueId);
}

public interface IEventRepository : IRepository<Event>
{
    Task<IEnumerable<Event>> GetEventsByStructureAsync(int structureId);
    Task<IEnumerable<Event>> GetActualEventsAsync();
}

public interface IEventSubscriptionRepository
{
    Task<IEnumerable<EventSubscription>> GetByEventAsync(int eventId);
    Task<EventSubscription?> GetAsync(int eventId, int itemId);
    Task<EventSubscription> AddAsync(EventSubscription subscription);
    Task DeleteAsync(EventSubscription subscription);
    Task SaveChangesAsync();
}

public interface IUserStructureRepository
{
    Task<UserStructure?> GetAsync(int userId, int structureId);
    Task<IEnumerable<UserStructure>> GetByUserAsync(int userId);
    Task AddAsync(UserStructure userStructure);
    Task DeleteAsync(UserStructure userStructure);
    Task SaveChangesAsync();
}

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task DeleteExpiredTokensAsync();
}
