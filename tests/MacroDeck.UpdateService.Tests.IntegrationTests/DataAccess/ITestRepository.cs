using MacroDeck.UpdateService.Core.DataAccess.Entities;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess;

public interface ITestRepository<T>
    where T : BaseEntity
{
    public IQueryable<T> AsQueryable();
    public Task InsertAsync(T obj);
    public Task UpdateAsync(T obj);
    public Task DeleteAsync(int id);
}