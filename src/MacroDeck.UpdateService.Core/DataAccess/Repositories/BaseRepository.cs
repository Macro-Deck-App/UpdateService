using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MacroDeck.UpdateService.Core.DataAccess.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
    where T : BaseEntity
{
    private readonly ILogger _logger = Log.ForContext<T>();
    
    protected UpdateServiceContext Context { get; }

    protected BaseRepository(UpdateServiceContext context)
    {
        Context = context;
        Context.Database.EnsureCreated();
    }

    public async Task InsertAsync(T obj)
    {
        await Context.Set<T>().AddAsync(obj);
        await SaveAsync();
    }

    public async Task UpdateAsync(T obj)
    {
        Context.Entry(obj).State = EntityState.Modified;
        await SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await Context.Set<T>().FindAsync(id);
        if (existing != null)
        {
            Context.Set<T>().Remove(existing);
        }
    }

    public async Task SaveAsync()
    {
        try
        {
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Error while updating entity {Type}", nameof(T));
        }
        finally
        {
            Context.ChangeTracker.Clear();
        }
    }
}