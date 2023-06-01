using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MacroDeck.UpdateService.Core.DataAccess;

public class UpdateServiceContext : DbContext
{
    public UpdateServiceContext(DbContextOptions<UpdateServiceContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var applyGenericMethod =
            typeof(ModelBuilder).GetMethod("ApplyConfiguration", BindingFlags.Instance | BindingFlags.Public);
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                     .Where(c => c is { IsClass: true, IsAbstract: false, ContainsGenericParameters: false })) 
        {
            foreach (var i in type.GetInterfaces())
            {
                if (!i.IsConstructedGenericType 
                    || i.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>)) continue;
                
                var applyConcreteMethod = applyGenericMethod?.MakeGenericMethod(i.GenericTypeArguments[0]);
                applyConcreteMethod?.Invoke(modelBuilder, new []
                {
                    Activator.CreateInstance(type)
                });
                break;
            }
        }
    }
}