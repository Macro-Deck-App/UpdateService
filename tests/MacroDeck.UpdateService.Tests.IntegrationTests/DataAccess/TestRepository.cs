using MacroDeck.UpdateService.Core.DataAccess;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace MacroDeck.UpdateService.Tests.IntegrationTests.DataAccess;

public class TestRepository<T> : ITestRepository<T>
    where T : BaseEntity
{
    private readonly UpdateServiceContext _context;

    public TestRepository(UpdateServiceContext context)
    {
        _context = context;
    }

    public IQueryable<T> AsQueryable()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public async Task InsertAsync(T obj)
    {
        await _context.AddAsync(obj);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T obj)
    {
        _context.Entry(obj).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Set<T>().FindAsync(id);
        if (existing != null)
        {
            _context.Set<T>().Remove(existing);
        }
    }
}