using Microsoft.Extensions.DependencyInjection;
using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Factories;
using TextLifeRpg.Application.Randomization;
using TextLifeRpg.Application.RelationshipStrategies;
using TextLifeRpg.Application.Services;

namespace TextLifeRpg.Application;

/// <summary>
/// Extension methods for configuring application layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
  #region Methods

  /// <summary>
  /// Registers application services in the DI container.
  /// </summary>
  /// <param name="services">The service collection to configure.</param>
  public static void AddApplication(this IServiceCollection services)
  {
    services.AddScoped<IRandomProvider, RandomProvider>();
    services.AddScoped<ICharacterPairSelector, RandomPairSelector>();
    services.AddScoped<IRelationshipFactory, RelationshipFactory>();
    services.AddScoped<IRelationshipRule, FriendshipRule>();
    services.AddScoped<IRelationshipRule, EnemyRule>();
    services.AddScoped<IRelationshipRule, RomanticPartnerRule>();
    services.AddScoped<IRelationshipRule, SpouseRule>();
    services.AddScoped<IRelationshipRule, CasualRomanticPartnerRule>();
    services.AddScoped<IRelationshipService, RelationshipService>();
    services.AddScoped<IScheduleService, ScheduleService>();
    services.AddScoped<ITraitService, TraitService>();
    services.AddScoped<ILocationService, LocationService>();
    services.AddScoped<IRoomService, RoomService>();
    services.AddScoped<ISaveService, SaveService>();
    services.AddScoped<IWorldService, WorldService>();
    services.AddScoped<ICharacterService, CharacterService>();
    services.AddScoped<IMovementService, MovementService>();
    services.AddScoped<IMovementNarrationService, MovementNarrationService>();
    services.AddScoped<INarrationService, NarrationService>();
    services.AddScoped<IExplorationActionService, ExplorationActionService>();
    services.AddScoped<IExplorationActionResultService, ExplorationActionResultService>();
    services.AddScoped<IExplorationActionResultNarrationService, ExplorationActionResultNarrationService>();
    services.AddScoped<IJobService, JobService>();
    services.AddScoped<IItemService, ItemService>();
    services.AddScoped<IDialogueService, DialogueService>();
  }

  #endregion
}
