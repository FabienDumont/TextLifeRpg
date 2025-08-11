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
    AddGreeting("Oh. It’s you. Alright.", "Blunt", 0, 50);
    AddGreeting("Hey. You're cool. Don't make it weird.", "Blunt", 50, 100);

    AddGreeting("Ugh. You again.", "Mean", -100, -50);
    AddGreeting("Didn’t think you'd show up. Shame.", "Mean", -50, 0);
    AddGreeting("You’re late. But whatever.", "Mean", 0, 50);
    AddGreeting("Hey. I guess you're not totally useless.", "Mean", 50, 100);

    AddGreeting("Whoa! You again?", "Outgoing", -100, -50);
    AddGreeting("Oh hey, unexpected but welcome!", "Outgoing", -50, 0);
    AddGreeting("Yo! Been waiting for someone like you.", "Outgoing", 0, 50);
    AddGreeting("You’re here! Let’s make this day amazing!", "Outgoing", 50, 100);

    AddGreeting("Good day... I suppose.", "Polite", -100, -50);
    AddGreeting("Ah. Hello again.", "Polite", -50, 0);
    AddGreeting("I'm glad we crossed paths today.", "Polite", 0, 50);
    AddGreeting("Your presence is most welcome.", "Polite", 50, 100);

    AddGreeting("What are you even doing here?", "Rude", -100, -50);
    AddGreeting("Yeah, it's you. Great.", "Rude", -50, 0);
    AddGreeting("Didn’t expect to see you. Not in a bad way.", "Rude", 0, 50);
    AddGreeting("Hey. You're... not terrible.", "Rude", 50, 100);

    AddGreeting("Don't expect me to care.", "Selfish", -100, -50);
    AddGreeting("You're still around?", "Selfish", -50, 0);
    AddGreeting("I’ll say hi. Just this once.", "Selfish", 0, 50);
    AddGreeting("Alright fine, you’ve earned a 'hi'.", "Selfish", 50, 100);

    AddGreeting("…Oh. Hi.", "Shy", -100, -50);
    AddGreeting("Oh. Hello again…", "Shy", -50, 0);
    AddGreeting("I'm… glad you're here.", "Shy", 0, 50);
    AddGreeting("Hi! I was hoping you'd show up.", "Shy", 50, 100);

    // Fallback greetings (based only on relationship level)
    AddFallbackGreeting("Ugh... What now?", -100, -50);
    AddFallbackGreeting("Oh. It's you.", -50, 0);
    AddFallbackGreeting("Hey. Nice to see you.", 0, 50);
    AddFallbackGreeting("Hello! Glad you're here!", 50, 100);

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
