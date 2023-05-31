using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MacroDeck.UpdateService.Core.DataAccess.Extensions;

public static class UpdateContextExtensions
{
    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider,
        Action? runAfterMigration = null)
    {
        Log.Information("Starting database migration...");
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var macroDeckContext = scope.ServiceProvider.GetRequiredService<UpdateServiceContext>();

        await macroDeckContext.Database.MigrateAsync();
        await macroDeckContext.SaveChangesAsync();
        
        Log.Information("Database migration finished");
        runAfterMigration?.Invoke();
    }
}