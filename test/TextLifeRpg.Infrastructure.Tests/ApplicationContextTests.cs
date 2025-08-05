using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests;

public class ApplicationContextTests
{
  #region Fields

  private readonly DbContextOptions<ApplicationContext> _options = new DbContextOptionsBuilder<ApplicationContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

  #endregion

  #region Methods

  [Fact]
  public async Task InitializeDataAsync_ShouldNotSeed_WhenTraitsAlreadyExist()
  {
    // Arrange
    await using var context = new ApplicationContext(_options);
    context.Traits.Add(new TraitDataModel {Id = Guid.NewGuid(), Name = "Existing"});
    await context.SaveChangesAsync();

    // Act
    await context.InitializeDataAsync();

    // Assert
    var traits = await context.Traits.ToListAsync();
    Assert.Single(traits);
    Assert.Equal("Existing", traits[0].Name);

    var incompatibilities = await context.TraitIncompatibilities.ToListAsync();
    Assert.Empty(incompatibilities);

    var greetings = await context.Greetings.ToListAsync();
    Assert.Empty(greetings);

    var dialogueOptions = await context.DialogueOptions.ToListAsync();
    Assert.Empty(dialogueOptions);

    var locations = await context.Locations.ToListAsync();
    Assert.Empty(locations);

    var rooms = await context.Rooms.ToListAsync();
    Assert.Empty(rooms);
  }

  [Fact]
  public async Task InitializeDataAsync_ShouldSeedMinimumData()
  {
    // Arrange
    await using var context = new ApplicationContext(_options);

    // Act
    await context.InitializeDataAsync();

    // Assert
    Assert.NotEmpty(await context.Traits.ToListAsync());
    Assert.NotEmpty(await context.TraitIncompatibilities.ToListAsync());
    Assert.NotEmpty(await context.Greetings.ToListAsync());
    Assert.NotEmpty(await context.DialogueOptions.ToListAsync());
    Assert.NotEmpty(await context.Locations.ToListAsync());
    Assert.NotEmpty(await context.Rooms.ToListAsync());
    Assert.NotEmpty(await context.Jobs.ToListAsync());
  }

  #endregion
}
