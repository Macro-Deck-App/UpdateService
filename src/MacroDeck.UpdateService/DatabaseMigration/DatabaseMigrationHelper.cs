using EvolveDb;
using MacroDeck.UpdateService.Core.Configuration;
using Npgsql;
using Serilog;

namespace MacroDeck.UpdateService.DatabaseMigration;

public class DatabaseMigrationHelper
{
    public static void MigrateDatabase()
    {
        try
        {
            var connection = new NpgsqlConnection(UpdateServiceConfiguration.DatabaseConnectionString);
            var evolve = new Evolve(connection, Log.Information)
            {
                Locations = new[] { "DatabaseMigration/Migrations" },
                IsEraseDisabled = true,
                Schemas = new []{ "evolve" }
            };

            evolve.Migrate();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Database migration failed");
            throw;
        }
    }
}