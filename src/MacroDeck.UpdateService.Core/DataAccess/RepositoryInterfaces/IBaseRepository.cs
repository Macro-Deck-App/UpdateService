using MacroDeck.UpdateService.Core.DataAccess.Entities;

namespace MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;

public interface IBaseRepository<T> 
    where T : BaseEntity
{
    public Task InsertAsync(T obj);
    public Task UpdateAsync(T obj);
    public Task DeleteAsync(int id);
    public Task SaveAsync();
}