namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing movement narrations.
/// </summary>
public interface IMovementNarrationService
{
  #region Methods

  /// <summary>
  /// Retrieves the narration text associated with a specific movement.
  /// </summary>
  /// <param name="movementId">The unique identifier of the movement.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The narration text for the specified movement.</returns>
  Task<string> GetNarrationTextAsync(Guid movementId, CancellationToken cancellationToken);

  #endregion
}
