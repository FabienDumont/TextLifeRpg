using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.Tests.Helpers;

public class ConditionEvaluatorTests
{
  #region Methods

  [Fact]
  public void EvaluateCondition_HasTrait_Matches_ReturnsTrue()
  {
    // Arrange
    var traitId = Guid.NewGuid();
    var condition = new ConditionDataModel
    {
      ConditionType = ConditionType.ActorHasTrait,
      OperandLeft = traitId.ToString(),
      Operator = "=",
      OperandRight = "true",
      Negate = false,
      ContextType = ContextType.Greeting
    };

    var character = new CharacterBuilder().Build();
    character.AddTraits([traitId]);
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    // Act
    var result = ConditionEvaluator.EvaluateCondition(condition, gameContext);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void EvaluateCondition_HasTrait_NotPresent_ReturnsFalse()
  {
    // Arrange
    var condition = new ConditionDataModel
    {
      ConditionType = ConditionType.ActorHasTrait,
      OperandLeft = Guid.NewGuid().ToString(),
      Operator = "=",
      OperandRight = "true",
      Negate = false,
      ContextType = ContextType.Greeting
    };

    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    // Act
    var result = ConditionEvaluator.EvaluateCondition(condition, gameContext);

    // Assert
    Assert.False(result);
  }

  [Fact]
  public void EvaluateCondition_HasTrait_Negated_ReturnsTrueWhenTraitNotPresent()
  {
    // Arrange
    var condition = new ConditionDataModel
    {
      ConditionType = ConditionType.ActorHasTrait,
      OperandLeft = Guid.NewGuid().ToString(),
      Operator = "=",
      OperandRight = "true",
      Negate = true,
      ContextType = ContextType.Greeting
    };

    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    // Act
    var result = ConditionEvaluator.EvaluateCondition(condition, gameContext);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void EvaluateCondition_UnknownOperator_ReturnsFalse()
  {
    // Arrange
    var condition = new ConditionDataModel
    {
      ConditionType = ConditionType.ActorEnergy,
      Operator = "??", // invalid operator
      OperandRight = "50",
      Negate = false,
      ContextType = ContextType.Greeting
    };

    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    // Act
    var result = ConditionEvaluator.EvaluateCondition(condition, gameContext);

    // Assert
    Assert.False(result);
  }

  [Theory]
  [InlineData("=", 50, "50", true)]
  [InlineData("!=", 50, "40", true)]
  [InlineData(">", 50, "40", true)]
  [InlineData("<", 50, "60", true)]
  [InlineData(">=", 50, "50", true)]
  [InlineData("<=", 50, "50", true)]
  [InlineData(">", 50, "60", false)]
  public void EvaluateCondition_Energy_Operators_Work(string op, int energy, string right, bool expected)
  {
    // Arrange
    var condition = new ConditionDataModel
    {
      ConditionType = ConditionType.ActorEnergy,
      Operator = op,
      OperandRight = right,
      Negate = false,
      ContextType = ContextType.Greeting
    };

    var character = new CharacterBuilder().Build();
    character.Energy = energy;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    // Act
    var result = ConditionEvaluator.EvaluateCondition(condition, gameContext);

    // Assert
    Assert.Equal(expected, result);
  }

  #endregion
}
