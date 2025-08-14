using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Tests.Seeders.Builders;

public class DialogueOptionResultBuilderTests
{
  #region Methods

  [Fact]
  public async Task BuildAsync_ShouldSaveResult_WithConditions_Texts_AndFollowUps()
  {
    // Arrange
    var dbName = Guid.NewGuid().ToString();
    var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: dbName).Options;

    await using var context = new ApplicationContext(options);

    // Seed required DialogueOptions (target option + 2 follow-ups)
    var optionId = Guid.NewGuid();
    var next1 = Guid.NewGuid();
    var next2 = Guid.NewGuid();

    context.DialogueOptions.AddRange(
      new DialogueOptionDataModel {Id = optionId, Label = "Ask something"},
      new DialogueOptionDataModel {Id = next1, Label = "Ask about job"},
      new DialogueOptionDataModel {Id = next2, Label = "Nevermind"}
    );
    await context.SaveChangesAsync();

    const int delta = 5;
    const string spokenYes = "Yes.";
    const string narrText = "She smiles.";
    const Fact fact = Fact.Job;
    var traitId = Guid.NewGuid();

    var builder = new DialogueOptionResultBuilder(context, optionId).WithTargetRelationshipValueChange(delta)
      .WithActorLearnFact(fact).EndDialogue().WithNextDialogueOptions([next1, next2]).WithActorTraitCondition(traitId)
      .WithActorRelationshipValueCondition(">=", "0").WithActorEnergyCondition("<", "30")
      .AddResultSpokenText(spokenYes, b => b.WithActorEnergyCondition(">", "40")).AddResultNarration(
        narrText, b => b.WithActorRelationshipValueCondition(">=", "10")
      );

    // Act
    await builder.BuildAsync();
    await context.SaveChangesAsync();

    // Assert: result saved
    var result = await context.DialogueOptionResults.SingleAsync();
    Assert.Equal(optionId, result.DialogueOptionId);
    Assert.Equal(delta, result.TargetRelationshipValueChange);
    Assert.Equal(fact, result.ActorLearnFact);
    Assert.True(result.EndDialogue);

    // Assert: follow-up links saved in order
    var links = await context.DialogueOptionResultNextDialogueOptions.Where(l => l.DialogueOptionResultId == result.Id)
      .OrderBy(l => l.Order).ToListAsync();

    Assert.Equal(2, links.Count);
    Assert.Equal(next1, links[0].NextDialogueOptionId);
    Assert.Equal(0, links[0].Order);
    Assert.Equal(next2, links[1].NextDialogueOptionId);
    Assert.Equal(1, links[1].Order);

    // Assert: RESULT-LEVEL conditions (ContextType = DialogueOptionResult, ContextId = result.Id)
    var resultConds = await context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResult && c.ContextId == result.Id).ToListAsync();

    Assert.Equal(3, resultConds.Count);
    Assert.Contains(
      resultConds, c => c.ConditionType == ConditionType.ActorHasTrait && c.OperandLeft == traitId.ToString()
    );
    Assert.Contains(
      resultConds, c => c is {ConditionType: ConditionType.ActorRelationship, Operator: ">=", OperandRight: "0"}
    );
    Assert.Contains(
      resultConds, c => c is {ConditionType: ConditionType.ActorEnergy, Operator: "<", OperandRight: "30"}
    );

    // Assert: spoken text saved and its condition bound to textId
    var spoken = await context.DialogueOptionResultSpokenTexts.SingleAsync();
    Assert.Equal(result.Id, spoken.DialogueOptionResultId);
    Assert.Equal(spokenYes, spoken.Text);

    var spokenConds = await context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResultSpokenText && c.ContextId == spoken.Id)
      .ToListAsync();

    Assert.Single(spokenConds);
    Assert.Equal(ConditionType.ActorEnergy, spokenConds[0].ConditionType);
    Assert.Equal(">", spokenConds[0].Operator);
    Assert.Equal("40", spokenConds[0].OperandRight);

    // Assert: narration saved and its condition bound to narrationId
    var narr = await context.DialogueOptionResultNarrations.SingleAsync();
    Assert.Equal(result.Id, narr.DialogueOptionResultId);
    Assert.Equal(narrText, narr.Text);

    var narrConds = await context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResultNarration && c.ContextId == narr.Id).ToListAsync();

    Assert.Single(narrConds);
    Assert.Equal(ConditionType.ActorRelationship, narrConds[0].ConditionType);
    Assert.Equal(">=", narrConds[0].Operator);
    Assert.Equal("10", narrConds[0].OperandRight);
  }

  #endregion
}
