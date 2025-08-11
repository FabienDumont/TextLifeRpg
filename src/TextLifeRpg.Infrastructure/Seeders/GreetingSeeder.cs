using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Greeting data seeder.
/// </summary>
public class GreetingSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    var traitMap = await context.Traits.ToDictionaryAsync(t => t.Name).ConfigureAwait(false);
    var greetings = new List<GreetingDataModel>();
    var conditions = new List<ConditionDataModel>();

    // Trait-based greetings
    AddGreeting("Tch. Not you again...", "Blunt", -100, -50);
    AddGreeting("What do you want now?", "Blunt", -50, 0);
    AddGreeting("Oh. It's you. Alright.", "Blunt", 0, 25);
    AddGreeting("You're here. Good.", "Blunt", 25, 50);
    AddGreeting("Hey. You're cool. Don't make it weird.", "Blunt", 50, 75);
    AddGreeting("There you are. I'm with you.", "Blunt", 75, 100);
    AddGreeting("Ugh. You again.", "Mean", -100, -50);
    AddGreeting("Didn’t think you'd show up. Shame.", "Mean", -50, 0);
    AddGreeting("You’re late. But whatever.", "Mean", 0, 50);
    AddGreeting("Hey. I guess you're not totally useless.", "Mean", 50, 100);

    AddGreeting("Ugh. You again.", "Mean", -100, -50);
    AddGreeting("Didn't think you'd show. Shame.", "Mean", -50, 0);
    AddGreeting("You're late. Whatever.", "Mean", 0, 25);
    AddGreeting("Huh. You made it.", "Mean", 25, 50);
    AddGreeting("Hey. I guess you're not totally useless.", "Mean", 50, 75);
    AddGreeting("If you're in, I'm in. Try not to blow it.", "Mean", 75, 100);

    AddGreeting("...Yeah, no. Not today.", "Outgoing", -100, -50);
    AddGreeting("Oh hey—unexpected.", "Outgoing", -50, 0);
    AddGreeting("Hey there.", "Outgoing", 0, 25);
    AddGreeting("Hey! Good timing.", "Outgoing", 25, 50);
    AddGreeting("There you are! Perfect.", "Outgoing", 50, 75);
    AddGreeting("You're here! You made my day.", "Outgoing", 75, 100);

    AddGreeting("Good day... I suppose.", "Polite", -100, -50);
    AddGreeting("Ah. Hello again.", "Polite", -50, 0);
    AddGreeting("Hello.", "Polite", 0, 25);
    AddGreeting("I'm glad we crossed paths.", "Polite", 25, 50);
    AddGreeting("Your presence is most welcome.", "Polite", 50, 75);
    AddGreeting("If you require anything, say so.", "Polite", 75, 100);

    AddGreeting("What are you even doing here?", "Rude", -100, -50);
    AddGreeting("Yeah, it's you. Great.", "Rude", -50, 0);
    AddGreeting("Huh. You again.", "Rude", 0, 25);
    AddGreeting("Fine. Come on.", "Rude", 25, 50);
    AddGreeting("You're... not terrible.", "Rude", 50, 75);
    AddGreeting("Finally. Let's get moving.", "Rude", 75, 100);

    AddGreeting("Don't expect me to care.", "Selfish", -100, -50);
    AddGreeting("You're still around?", "Selfish", -50, 0);
    AddGreeting("I'll say hi. Just this once.", "Selfish", 0, 25);
    AddGreeting("Alright, you've got my attention.", "Selfish", 25, 50);
    AddGreeting("If this benefits me, I'm in.", "Selfish", 50, 75);
    AddGreeting("For you, I'll make time.", "Selfish", 75, 100);

    AddGreeting("...Oh. Hi.", "Shy", -100, -50);
    AddGreeting("Oh. Hello again...", "Shy", -50, 0);
    AddGreeting("Hi.", "Shy", 0, 25);
    AddGreeting("I'm... glad you're here.", "Shy", 25, 50);
    AddGreeting("Hi! I was hoping you'd show up.", "Shy", 50, 75);
    AddGreeting("I—I hoped you'd come back.", "Shy", 75, 100);

    // Fallback greetings (relationship-only)
    AddFallbackGreeting("Ugh... What now?", -100, -50);
    AddFallbackGreeting("Oh. It's you.", -50, 0);
    AddFallbackGreeting("Hey.", 0, 25);
    AddFallbackGreeting("Hey. Nice to see you.", 25, 50);
    AddFallbackGreeting("Hello! Glad you're here!", 50, 75);
    AddFallbackGreeting("It's good to see you again.", 75, 100);

    await context.Greetings.AddRangeAsync(greetings).ConfigureAwait(false);
    await context.Conditions.AddRangeAsync(conditions).ConfigureAwait(false);
    await context.SaveChangesAsync().ConfigureAwait(false);
    return;

    void AddGreeting(string text, string trait, int min, int max)
    {
      var greeting = new GreetingDataModel {Id = Guid.NewGuid(), SpokenText = text};
      greetings.Add(greeting);

      var traitId = traitMap[trait].Id;
      conditions.AddRange(
        ConditionBuilder.BuildTraitConditions(ContextType.Greeting, greeting.Id, ConditionType.ActorHasTrait, [traitId])
      );

      AddRelationshipConditions(greeting.Id, min, max);
    }

    void AddFallbackGreeting(string text, int min, int max)
    {
      var greeting = new GreetingDataModel {Id = Guid.NewGuid(), SpokenText = text};
      greetings.Add(greeting);
      AddRelationshipConditions(greeting.Id, min, max);
    }

    void AddRelationshipConditions(Guid greetingId, int min, int max)
    {
      conditions.Add(
        new ConditionDataModel
        {
          Id = Guid.NewGuid(),
          ContextType = ContextType.Greeting,
          ContextId = greetingId,
          ConditionType = ConditionType.ActorRelationship,
          Operator = ">=",
          OperandRight = min.ToString(),
          Negate = false
        }
      );

      conditions.Add(
        new ConditionDataModel
        {
          Id = Guid.NewGuid(),
          ContextType = ContextType.Greeting,
          ContextId = greetingId,
          ConditionType = ConditionType.ActorRelationship,
          Operator = "<",
          OperandRight = max.ToString(),
          Negate = false
        }
      );
    }
  }

  #endregion
}
