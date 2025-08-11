using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for dialogue option results.
/// </summary>
public interface IDialogueOptionResultRepository
{
  #region Methods

  /// <summary>
  /// Retrieves dialogue option result for given dialogue option identifier and game context.
  /// </summary>
  Task<DialogueOptionResult> GetByDialogueOptionIdAsync(
    Guid dialogueOptionId, GameContext gameContext, CancellationToken cancellationToken
  );

  #endregion
}
