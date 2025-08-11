using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Infrastructure.EfRepositories;
using TextLifeRpg.Infrastructure.JsonRepositories;

namespace TextLifeRpg.Infrastructure.Tests;

public class ServiceCollectionExtensionsTests
{
  #region Methods

  [Fact]
  public async Task ConfigureApplicationContext_ShouldSetSqliteAndNoTracking()
  {
    // Arrange
    var dbFile = $"{Path.GetRandomFileName()}.db";
    var conn = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFile)};";
    var builder = new DbContextOptionsBuilder<ApplicationContext>();

    // Act
    ServiceCollectionExtensions.ConfigureApplicationContext(builder, conn);

    await using var ctx = new ApplicationContext(builder.Options);
    await ctx.Database.EnsureCreatedAsync();

    // Assert
    Assert.Equal(conn, ctx.Database.GetDbConnection().ConnectionString);
    Assert.Equal(QueryTrackingBehavior.NoTracking, ctx.ChangeTracker.QueryTrackingBehavior);
    Assert.Equal("Microsoft.EntityFrameworkCore.Sqlite", ctx.Database.ProviderName);
  }

  [Fact]
  public void AddInfrastructure_ShouldRegisterAllRepositories()
  {
    // Arrange
    var services = new ServiceCollection();

    // Act
    services.AddInfrastructure("test.db");
    var provider = services.BuildServiceProvider();

    // TraitRepository
    var traitRepo = provider.GetService<ITraitRepository>();
    Assert.NotNull(traitRepo);
    Assert.IsType<TraitRepository>(traitRepo);

    // GreetingRepository
    var greetingRepo = provider.GetService<IGreetingRepository>();
    Assert.NotNull(greetingRepo);
    Assert.IsType<GreetingRepository>(greetingRepo);

    // GameSaveJsonRepository
    var saveRepo = provider.GetService<IGameSaveRepository>();
    Assert.NotNull(saveRepo);
    Assert.IsType<GameSaveJsonRepository>(saveRepo);

    // NameJsonRepository
    var nameRepo = provider.GetService<INameRepository>();
    Assert.NotNull(nameRepo);
    Assert.IsType<NameJsonRepository>(nameRepo);

    // JobRepository
    var jobRepo = provider.GetService<IJobRepository>();
    Assert.NotNull(jobRepo);
    Assert.IsType<JobRepository>(jobRepo);

    // JobRepository
    var itemRepo = provider.GetService<IItemRepository>();
    Assert.NotNull(itemRepo);
    Assert.IsType<ItemRepository>(itemRepo);
  }

  #endregion
}
