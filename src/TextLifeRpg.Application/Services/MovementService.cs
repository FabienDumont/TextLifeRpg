using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for movements.
/// </summary>
public class MovementService(
  IMovementRepository movementRepository, IMovementNarrationRepository movementNarrationRepository,
  ILocationService locationService
) : IMovementService
{
  #region Implementation of IMovementService

  /// <inheritdoc />
  public async Task<List<Movement>> GetAvailableMovementsAsync(
    Guid currentLocationId, Guid? currentRoomId, DayOfWeek day, TimeSpan time, CancellationToken cancellationToken
  )
  {
    var movements = await movementRepository.GetMovementsAsync(currentLocationId, currentRoomId, cancellationToken);

    List<Movement> availableMovements = [];

    foreach (var movement in movements)
    {
      var locationDestinationId = movement.ToLocationId;

      if (await locationService.IsLocationOpenAsync(locationDestinationId, day, time, cancellationToken))
      {
        availableMovements.Add(movement);
      }
    }

    return availableMovements;
  }

  /// <inheritdoc />
  public async Task ExecuteAsync(Movement movement, GameSave save, CancellationToken cancellationToken)
  {
    save.ResetText();

    var player = save.PlayerCharacter;
    var world = save.World;

    player.MoveTo(movement.ToLocationId, movement.ToRoomId);

    world.AdvanceTime(1, player.Id);

    var narration =
      await movementNarrationRepository.GetMovementNarrationFromMovementIdAsync(movement.Id, cancellationToken);

    TextLineBuilder.BuildNarrationLine(narration, player, null, save);
  }

  #endregion
}
