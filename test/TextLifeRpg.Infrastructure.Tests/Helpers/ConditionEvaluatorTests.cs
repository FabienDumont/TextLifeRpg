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

  [Theory]
  [InlineData(ConditionType.ActorEnergy, "=", 50, "50", true)]
  [InlineData(ConditionType.ActorEnergy, "!=", 50, "40", true)]
  [InlineData(ConditionType.ActorEnergy, ">", 50, "40", true)]
  [InlineData(ConditionType.ActorEnergy, "<", 50, "60", true)]
  [InlineData(ConditionType.ActorEnergy, ">=", 50, "50", true)]
  [InlineData(ConditionType.ActorEnergy, "<=", 50, "50", true)]
  [InlineData(ConditionType.ActorEnergy, ">", 50, "60", false)]
  [InlineData(ConditionType.ActorMoney, "=", 50, "50", true)]
  [InlineData(ConditionType.ActorMoney, "!=", 50, "40", true)]
  [InlineData(ConditionType.ActorMoney, ">", 50, "40", true)]
  [InlineData(ConditionType.ActorMoney, "<", 50, "60", true)]
  [InlineData(ConditionType.ActorMoney, ">=", 50, "50", true)]
  [InlineData(ConditionType.ActorMoney, "<=", 50, "50", true)]
  [InlineData(ConditionType.ActorMoney, ">", 50, "60", false)]
  public void EvaluateCondition_Energy_Operators_Work(
    ConditionType conditionType, string op, int value, string right, bool expected
  )
  {
    // Arrange
    var condition = new ConditionDataModel
    {
      ConditionType = conditionType,
      Operator = op,
      OperandRight = right,
      Negate = false,
      ContextType = ContextType.Greeting
    };

    var character = new CharacterBuilder().Build();
    switch (conditionType)
    {
      case ConditionType.ActorEnergy:
        character.Energy = value;
        break;
      case ConditionType.ActorMoney:
        character.Money = value;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(conditionType), conditionType, null);
    }

    character.Energy = value;
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

  [Theory]
  [InlineData(ConditionType.ActorEnergy)]
  [InlineData(ConditionType.ActorMoney)]
  [InlineData(ConditionType.ActorRelationship)]
  public void EvaluateCondition_MissingOperandRight_ThrowsException(ConditionType type)
  {
    // Arrange
    var actor = new CharacterBuilder().Build();
    var target = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [actor, target]);

    var condition = new ConditionDataModel
    {
      ConditionType = type,
      Operator = ">",
      OperandRight = null, // This triggers the exception
      ContextType = ContextType.Greeting
    };

    var context = new GameContext
    {
      Actor = actor,
      Target = target,
      World = world
    };

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => ConditionEvaluator.EvaluateCondition(condition, context));
  }

  [Fact]
  public void EvaluateCondition_UnknownOperator_ThrowsException()
  {
    // Arrange
    var target = new CharacterBuilder().Build();
    var actor = new CharacterBuilder().Build();

    var condition = new ConditionDataModel
    {
      ConditionType = ConditionType.ActorEnergy,
      Operator = ",,",
      OperandRight = "50",
      ContextType = ContextType.Greeting
    };

    var world = World.Create(DateTime.Now, [actor, target]);

    var gameContext = new GameContext
    {
      Actor = actor,
      Target = target,
      World = world
    };

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => ConditionEvaluator.EvaluateCondition(condition, gameContext));
  }

  [Fact]
  public void EvaluateCondition_InvalidConditionType_ThrowsException()
  {
    // Arrange
    var target = new CharacterBuilder().Build();
    var actor = new CharacterBuilder().Build();

    var condition = new ConditionDataModel
    {
      ConditionType = (ConditionType) 999,
      Operator = "==",
      OperandRight = "50",
      ContextType = ContextType.Greeting
    };

    var world = World.Create(DateTime.Now, [actor, target]);

    var gameContext = new GameContext
    {
      Actor = actor,
      Target = target,
      World = world
    };

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => ConditionEvaluator.EvaluateCondition(condition, gameContext));
  }

  [Fact]
  public void EvaluateCondition_ActorRelationship_MatchesCondition_ReturnsTrue()
  {
    // Arrange
    var actor = new CharacterBuilder().Build();
    var target = new CharacterBuilder().Build();

    const int relationshipValue = 75;

    var condition = new ConditionDataModel
    {
      ConditionType = ConditionType.ActorRelationship,
      Operator = ">",
      OperandRight = "50",
      ContextType = ContextType.Greeting
    };

    var world = World.Create(DateTime.Now, [actor, target]);
    var now = new DateOnly(2025, 6, 16);
    world.Relationships.Add(
      Relationship.Create(actor.Id, target.Id, RelationshipType.Acquaintance, now, now, relationshipValue)
    );

    var gameContext = new GameContext
    {
      Actor = actor,
      Target = target,
      World = world
    };

    // Act
    var result = ConditionEvaluator.EvaluateCondition(condition, gameContext);

    // Assert
    Assert.True(result);
  }

  #endregion
}
