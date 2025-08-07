using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Dialogue option data seeder.
/// </summary>
public class DialogueOptionSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    var traitMap = await context.Traits.ToDictionaryAsync(t => t.Name).ConfigureAwait(false);

    await new DialogueOptionBuilder(context, "Say goodbye").EndDialogue().AddSpokenText("Goodbye.", _ => { })
      .AddResultSpokenText(
        "Yeah fuck off, I hate you.",
        b => b.WithActorTraitCondition(traitMap["Blunt"].Id).WithActorRelationshipValueCondition("<", "-50")
      )
      .AddResultSpokenText(
        "Don't bother next time.",
        b => b.WithActorTraitCondition(traitMap["Blunt"].Id).WithActorRelationshipValueCondition("<", "0")
      )
      .AddResultSpokenText(
        "Bye.", b => b.WithActorTraitCondition(traitMap["Blunt"].Id).WithActorRelationshipValueCondition("<", "50")
      )
      .AddResultSpokenText(
        "It was nice talking to you. Bye.",
        b => b.WithActorTraitCondition(traitMap["Blunt"].Id).WithActorRelationshipValueCondition(">=", "50")
      )
      .AddResultSpokenText(
        "Take care... I guess everyone deserves a bit of kindness.",
        b => b.WithActorTraitCondition(traitMap["Kind"].Id).WithActorRelationshipValueCondition("<", "-50")
      )
      .AddResultSpokenText(
        "I hope your day improves, truly.",
        b => b.WithActorTraitCondition(traitMap["Kind"].Id).WithActorRelationshipValueCondition("<", "0")
      )
      .AddResultSpokenText(
        "Stay safe, okay?",
        b => b.WithActorTraitCondition(traitMap["Kind"].Id).WithActorRelationshipValueCondition("<", "50")
      )
      .AddResultSpokenText(
        "I'll miss you, I care so much about you!",
        b => b.WithActorTraitCondition(traitMap["Kind"].Id).WithActorRelationshipValueCondition(">=", "50")
      )
      .AddResultSpokenText(
        "Get lost, you parasite.",
        b => b.WithActorTraitCondition(traitMap["Mean"].Id).WithActorRelationshipValueCondition("<", "-50")
      )
      .AddResultSpokenText(
        "Bye. Don't come back.",
        b => b.WithActorTraitCondition(traitMap["Mean"].Id).WithActorRelationshipValueCondition("<", "0")
      )
      .AddResultSpokenText(
        "Whatever. Later.",
        b => b.WithActorTraitCondition(traitMap["Mean"].Id).WithActorRelationshipValueCondition("<", "50")
      )
      .AddResultSpokenText(
        "I tolerate you. That's rare. Goodbye.",
        b => b.WithActorTraitCondition(traitMap["Mean"].Id).WithActorRelationshipValueCondition(">=", "50")
      )
      .AddResultSpokenText(
        "Finally, we’re done. What a buzzkill.",
        b => b.WithActorTraitCondition(traitMap["Outgoing"].Id).WithActorRelationshipValueCondition("<", "-50")
      )
      .AddResultSpokenText(
        "Alright, I’m off! Try smiling sometime!",
        b => b.WithActorTraitCondition(traitMap["Outgoing"].Id).WithActorRelationshipValueCondition("<", "0")
      )
      .AddResultSpokenText(
        "Catch you later, maybe?",
        b => b.WithActorTraitCondition(traitMap["Outgoing"].Id).WithActorRelationshipValueCondition("<", "50")
      )
      .AddResultSpokenText(
        "Let’s hang again soon! You’re fun!",
        b => b.WithActorTraitCondition(traitMap["Outgoing"].Id).WithActorRelationshipValueCondition(">=", "50")
      ).AddResultSpokenText(
        "Farewell. I hope we never cross paths again.",
        b => b.WithActorTraitCondition(traitMap["Polite"].Id).WithActorRelationshipValueCondition("<", "-50")
      ).AddResultSpokenText(
        "Goodbye. I trust you’ll find your way.",
        b => b.WithActorTraitCondition(traitMap["Polite"].Id).WithActorRelationshipValueCondition("<", "0")
      ).AddResultSpokenText(
        "Take care. It was... fine.",
        b => b.WithActorTraitCondition(traitMap["Polite"].Id).WithActorRelationshipValueCondition("<", "50")
      ).AddResultSpokenText(
        "Wishing you the best. Until next time!",
        b => b.WithActorTraitCondition(traitMap["Polite"].Id).WithActorRelationshipValueCondition(">=", "50")
      ).AddResultSpokenText(
        "Don’t talk to me again.",
        b => b.WithActorTraitCondition(traitMap["Rude"].Id).WithActorRelationshipValueCondition("<", "-50")
      ).AddResultSpokenText(
        "Yeah, yeah, get outta here.",
        b => b.WithActorTraitCondition(traitMap["Rude"].Id).WithActorRelationshipValueCondition("<", "0")
      ).AddResultSpokenText(
        "Bye or whatever.",
        b => b.WithActorTraitCondition(traitMap["Rude"].Id).WithActorRelationshipValueCondition("<", "50")
      ).AddResultSpokenText(
        "You’re lucky I like you. Bye.",
        b => b.WithActorTraitCondition(traitMap["Rude"].Id).WithActorRelationshipValueCondition(">=", "50")
      ).AddResultSpokenText(
        "...Okay, I’m going now.",
        b => b.WithActorTraitCondition(traitMap["Shy"].Id).WithActorRelationshipValueCondition("<", "-50")
      ).AddResultSpokenText(
        "Bye...", b => b.WithActorTraitCondition(traitMap["Shy"].Id).WithActorRelationshipValueCondition("<", "0")
      ).AddResultSpokenText(
        "Bye. It was... um, nice.",
        b => b.WithActorTraitCondition(traitMap["Shy"].Id).WithActorRelationshipValueCondition("<", "50")
      ).AddResultSpokenText(
        "Thanks for today. I really enjoyed it.",
        b => b.WithActorTraitCondition(traitMap["Shy"].Id).WithActorRelationshipValueCondition(">=", "50")
      ).AddResultSpokenText("Alright, goodbye.", _ => { })
      .AddResultNarration("You walk away from [TARGETNAME].", _ => { }).BuildAsync();
  }

  #endregion
}
