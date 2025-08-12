using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Tests.Seeders.Builders;

public class TextVariantBuilderTests
{
  [Fact]
  public void Constructor_ShouldStoreText()
  {
    // Arrange
    var contextType = ContextType.DialogueOption;
    var contextId = Guid.NewGuid();
    var expectedText = "Hello there";

    // Act
    var builder = new TextVariantBuilder(contextType, contextId, expectedText);

    // Assert
    Assert.Equal(expectedText, builder.Text);
  }

  [Fact]
  public void WithEnergyCondition_ShouldAddCorrectCondition()
  {
    // Arrange
    var contextType = ContextType.DialogueOption;
    var contextId = Guid.NewGuid();

    // Act
    var builder = new TextVariantBuilder(contextType, contextId, "test").WithActorEnergyCondition(">", "50");

    var condition = builder.Conditions.Single();

    // Assert
    Assert.Equal(contextType, condition.ContextType);
    Assert.Equal(contextId, condition.ContextId);
    Assert.Equal(ConditionType.ActorEnergy, condition.ConditionType);
    Assert.Equal(">", condition.Operator);
    Assert.Equal("50", condition.OperandRight);
    Assert.False(condition.Negate);
  }

  [Fact]
  public void WithTraitCondition_ShouldAddCorrectCondition()
  {
    // Arrange
    var contextType = ContextType.ExplorationActionResult;
    var contextId = Guid.NewGuid();
    var traitId = Guid.NewGuid();

    // Act
    var builder = new TextVariantBuilder(contextType, contextId, "test").WithActorTraitCondition(traitId);

    var condition = builder.Conditions.Single();

    // Assert
    Assert.Equal(contextType, condition.ContextType);
    Assert.Equal(contextId, condition.ContextId);
    Assert.Equal(ConditionType.ActorHasTrait, condition.ConditionType);
    Assert.Equal(traitId.ToString(), condition.OperandLeft);
    Assert.Equal("true", condition.OperandRight);
    Assert.Equal("==", condition.Operator);
    Assert.False(condition.Negate);
  }

  [Fact]
  public void WithTraitCondition_WithNegate_ShouldSetNegateTrue()
  {
    // Arrange
    var contextType = ContextType.ExplorationActionResult;
    var contextId = Guid.NewGuid();
    var traitId = Guid.NewGuid();

    // Act
    var builder = new TextVariantBuilder(contextType, contextId, "test").WithActorTraitCondition(traitId, negate: true);

    var condition = builder.Conditions.Single();

    // Assert
    Assert.True(condition.Negate);
  }
}
