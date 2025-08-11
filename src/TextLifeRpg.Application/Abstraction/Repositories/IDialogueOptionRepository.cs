using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for dialogue options.
/// </summary>
public interface IDialogueOptionRepository
{
  #region Methods

  /// <summary>
  /// Retrieves initial dialogue options depending on the given game context.
  /// </summary>
  Task<IReadOnlyCollection<DialogueOption>> GetPossibleInitialDialogueOptionsAsync(
    GameContext gameContext, CancellationToken cancellationToken
  );

  /// <summary>
  /// Retrieves the possible follow-up dialogue options based on the given game context and a specific
  /// dialogue option result.
  /// </summary>
  /// <param name="context">The current game context, including the actor, world state, and target character.</param>
  /// <param name="dialogueOptionResultId">
  /// The unique identifier of the dialogue option result for which follow-ups are being retrieved.
  /// </param>
  /// <param name="ct">The cancellation token to observe for operation cancellation.</param>
  /// <returns>A read-only collection of dialogue options that represent the possible follow-ups.</returns>
  Task<IReadOnlyCollection<DialogueOption>> GetPossibleFollowUpsAsync(
    GameContext context, Guid dialogueOptionResultId, CancellationToken ct
  );

  #endregion
}
