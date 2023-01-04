using BlazorServerEFCoreSample.Data;
using Microsoft.EntityFrameworkCore;

public static class DatabaseUtility
{
    /// <summary>
    /// Method to see the database. Should not be used in production: demo purposes only.
    /// </summary>
    /// <param name="options">The configured options.</param>
    /// <param name="count">The number of contacts to seed.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public static async Task EnsureDbCreatedAndSeedWithCountOfAsync(DbContextOptions<ContactContext> options, int count)
    {
        // empty to avoid logging while inserting (otherwise will flood console)
        var factory = new LoggerFactory();
        var builder = new DbContextOptionsBuilder<ContactContext>(options)
            .UseLoggerFactory(factory);

        await using var context = new ContactContext(builder.Options);
        // result is true if the database had to be created
        if (await context.Database.EnsureCreatedAsync().ConfigureAwait(false))
        {
            var seed = new SeedContacts();
            await seed.SeedDatabaseWithContactCountOfAsync(context, count);
        }
    }
}
