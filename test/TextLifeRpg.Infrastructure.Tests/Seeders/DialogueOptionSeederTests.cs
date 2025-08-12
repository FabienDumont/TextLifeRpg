using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.JsonDefinitions;
using TextLifeRpg.Infrastructure.Seeders;
using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Tests.Seeders;

public class DialogueOptionSeederTests
{
  #region Methods

  [Fact]
  public async Task ApplyResultConditions_ShouldPersistResultLevelConditions()
  {
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    await using var context = new ApplicationContext(options);

    var optionId = Guid.NewGuid();
    context.DialogueOptions.Add(new DialogueOptionDataModel {Id = optionId, Label = "Ask something"});
    await context.SaveChangesAsync();

    var traitId = Guid.NewGuid();
    var traitMap = new Dictionary<string, Guid>(StringComparer.Ordinal)
    {
      ["Friendly"] = traitId
    };

    var defs = new List<DialogueOptionConditionDefinition>
    {
      new() {ActorHasTrait = "Friendly"}, // should be applied
      new() {ActorHasTrait = "Unknown"}, // should be ignored (not in map)
      new()
      {
        ActorRelationshipValue = new ConditionComparisonDefinition {Operator = ">=", Value = 0}
      },
      new()
      {
        ActorEnergy = new ConditionComparisonDefinition {Operator = "<", Value = 30}
      }
    };

    var rb = new DialogueOptionResultBuilder(context, optionId);

    // Use reflection to call private static method
    var mi = typeof(DialogueOptionSeeder).GetMethod(
      "ApplyResultConditions", BindingFlags.Static | BindingFlags.NonPublic
    );
    Assert.NotNull(mi);

    // Act
    mi.Invoke(null, [rb, defs, traitMap]);
    await rb.BuildAsync();
    await context.SaveChangesAsync();

    // Assert: one result created
    var result = await context.DialogueOptionResults.SingleAsync();
    Assert.Equal(optionId, result.DialogueOptionId);

    // Assert: RESULT-level conditions attached to result.Id
    var conds = await context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResult && c.ContextId == result.Id).ToListAsync();

    // Expect 3: trait(Friendly), relationship(>=0), energy(<30)
    Assert.Equal(3, conds.Count);

    Assert.Contains(
      conds,
      c => c.ConditionType == ConditionType.ActorHasTrait && c.Operator == "==" &&
           c.OperandLeft == traitId.ToString() && c.OperandRight == "true" && !c.Negate
    );

    Assert.Contains(
      conds, c => c.ConditionType == ConditionType.ActorRelationship && c.Operator == ">=" && c.OperandRight == "0"
    );

    Assert.Contains(
      conds, c => c.ConditionType == ConditionType.ActorEnergy && c.Operator == "<" && c.OperandRight == "30"
    );
  }

  [Fact]
  public async Task ApplyResultConditions_ShouldDoNothing_WhenListIsEmpty()
  {
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    await using var context = new ApplicationContext(options);

    var optionId = Guid.NewGuid();
    context.DialogueOptions.Add(new DialogueOptionDataModel {Id = optionId, Label = "Chit chat"});
    await context.SaveChangesAsync();

    var rb = new DialogueOptionResultBuilder(context, optionId);
    var traitMap = new Dictionary<string, Guid>();
    var defs = new List<DialogueOptionConditionDefinition>();

    var mi = typeof(DialogueOptionSeeder).GetMethod(
      "ApplyResultConditions", BindingFlags.Static | BindingFlags.NonPublic
    );
    Assert.NotNull(mi);

    // Act
    mi.Invoke(null, [rb, defs, traitMap]);
    await rb.BuildAsync();
    await context.SaveChangesAsync();

    // Assert
    var result = await context.DialogueOptionResults.SingleAsync();
    var conds = await context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResult && c.ContextId == result.Id).ToListAsync();

    Assert.Empty(conds);
  }

  #endregion
}
