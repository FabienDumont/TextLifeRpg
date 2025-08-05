using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for exploration actions.
/// </summary>
public class ExplorationActionService(
  IExplorationActionRepository explorationActionRepository,
  IExplorationActionResultRepository explorationActionResultRepository,
  IExplorationActionResultNarrationRepository explorationActionResultNarrationRepository,
  ILocationService locationService
) : IExplorationActionService
{
  #region Methods

  /// <summary>
  /// Determines the next day of the week, taking into account that Sunday wraps around to Monday.
  /// </summary>
  /// <param name="day">The current day of the week to determine the next day from.</param>
  /// <returns>The next day of the week.</returns>
  private static DayOfWeek GetNextDay(DayOfWeek day)
  {
    return day == DayOfWeek.Sunday ? DayOfWeek.Monday : (DayOfWeek) (((int) day + 1) % 7);
  }

  #endregion

  #region Implementation of IExplorationActionService

  /// <inheritdoc />
  public async Task<List<ExplorationAction>> GetExplorationActionsAsync(
    Guid locationId, Guid? roomId, DayOfWeek currentDay, TimeSpan currentTime, CancellationToken cancellationToken
  )
  {
    var location = await locationService.GetByIdAsync(locationId, cancellationToken);

    var explorationActions = await explorationActionRepository.GetByLocationAndRoomIdAsync(
      locationId, roomId, cancellationToken
    );

    if (location.IsAlwaysOpen)
    {
      return explorationActions;
    }

    var filtered = new List<ExplorationAction>();

    foreach (var action in explorationActions)
    {
      var timeAfterAction = currentTime.Add(TimeSpan.FromMinutes(action.NeededMinutes));

      var stillOpen = await locationService.IsLocationOpenAsync(
        locationId, timeAfterAction.Hours < currentTime.Hours ? GetNextDay(currentDay) : currentDay, timeAfterAction,
        cancellationToken
      );

      if (stillOpen)
      {
        filtered.Add(action);
      }
    }

    return filtered;
  }

  /// <inheritdoc />
  public async Task ExecuteAsync(ExplorationAction action, GameSave save, CancellationToken cancellationToken)
  {
    var character = save.PlayerCharacter;

    // Get action result and narration
    var gameContext = new GameContext
    {
      Actor = character,
      World = save.World
    };

    var result = await explorationActionResultRepository.GetByExplorationActionIdAsync(
      action.Id, gameContext, cancellationToken
    );

    var narration = await explorationActionResultNarrationRepository.GetByExplorationActionResultIdAsync(
      result.Id, gameContext, cancellationToken
    );

    // Apply effects
    if (result.AddMinutes)
    {
      save.World.AdvanceTime(action.NeededMinutes, save.PlayerCharacterId);
    }

    if (result.EnergyChange is not null)
    {
      character.Energy += result.EnergyChange.Value;
    }

    if (result.MoneyChange is not null)
    {
      character.Money += result.MoneyChange.Value;
    }

    var line = TextLineBuilder.BuildNarrationLine(narration, save.PlayerCharacter, save.PlayerCharacterId);

    save.AddText(line.TextParts);
  }

  #endregion
}
