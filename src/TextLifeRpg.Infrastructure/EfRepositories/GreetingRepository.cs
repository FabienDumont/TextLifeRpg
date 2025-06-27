using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for greetings.
/// </summary>
public class GreetingRepository(ApplicationContext context) : RepositoryBase(context), IGreetingRepository
{
  #region Implementation of IGreetingRepository

  /// <inheritdoc />
  public async Task<Greeting> GetAsync(GameContext gameContext, CancellationToken cancellationToken)
  {
    var allGreetings = await Context.Greetings.ToListAsync(cancellationToken);

    var allConditions = await Context.Conditions.Where(c => c.ContextType == ContextType.Greeting)
      .ToListAsync(cancellationToken);

    foreach (var greeting in allGreetings)
    {
      var conditions = allConditions.Where(c => c.ContextId == greeting.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return greeting.ToDomain();
      }
    }

    throw new InvalidOperationException("No appropriate greeting found.");
  }

  #endregion
}
