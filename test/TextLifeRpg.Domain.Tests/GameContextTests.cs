using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class GameContextTests
{
  #region Methods

  [Fact]
  public void Constructor_ShouldAssignPropertiesCorrectly()
  {
    // Arrange
    var actor  = new CharacterBuilder().Build();
    var target = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [actor, target]);

    // Act
    var context = new GameContext
    {
      Actor = actor,
      Target = target,
      World = world
    };

    // Assert
    Assert.Equal(actor, context.Actor);
    Assert.Equal(target, context.Target);
    Assert.Equal(world, context.World);
  }

  [Fact]
  public void Target_CanBeNull()
  {
    // Arrange
    var actor = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [actor]);

    // Act
    var context = new GameContext
    {
      Actor = actor,
      World = world
    };

    // Assert
    Assert.Equal(actor, context.Actor);
    Assert.Equal(world, context.World);
    Assert.Null(context.Target);
  }

  #endregion
}
