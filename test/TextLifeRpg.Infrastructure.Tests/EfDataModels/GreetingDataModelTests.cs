using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class ConditionDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithAllValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var contextId = Guid.NewGuid();

    const string operandLeft = "trait-id";
    const string @operator = "=";
    const string operandRight = "true";
    const bool negate = true;

    // Act
    var condition = new ConditionDataModel
    {
      Id = id,
      ContextType = ContextType.Greeting,
      ContextId = contextId,
      ConditionType = ConditionType.ActorHasTrait,
      OperandLeft = operandLeft,
      Operator = @operator,
      OperandRight = operandRight,
      Negate = negate
    };

    // Assert
    Assert.Equal(id, condition.Id);
    Assert.Equal(ContextType.Greeting, condition.ContextType);
    Assert.Equal(contextId, condition.ContextId);
    Assert.Equal(ConditionType.ActorHasTrait, condition.ConditionType);
    Assert.Equal(operandLeft, condition.OperandLeft);
    Assert.Equal(@operator, condition.Operator);
    Assert.Equal(operandRight, condition.OperandRight);
    Assert.True(condition.Negate);
  }

  [Fact]
  public void Instantiation_ShouldAllowNullOperands()
  {
    // Arrange
    var id = Guid.NewGuid();
    var contextId = Guid.NewGuid();

    // Act
    var condition = new ConditionDataModel
    {
      Id = id,
      ContextType = ContextType.Greeting,
      ContextId = contextId,
      ConditionType = ConditionType.ActorRelationship,
      Operator = ">=",
      OperandLeft = null,
      OperandRight = null,
      Negate = false
    };

    // Assert
    Assert.Equal(ContextType.Greeting, condition.ContextType);
    Assert.Null(condition.OperandLeft);
    Assert.Null(condition.OperandRight);
    Assert.False(condition.Negate);
  }

  #endregion
}
