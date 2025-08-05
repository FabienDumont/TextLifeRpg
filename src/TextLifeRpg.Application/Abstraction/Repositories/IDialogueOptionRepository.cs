using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for dialogue options.
/// </summary>
public interface IDialogueOptionRepository
{
  #region Methods

  /// <summary>
  /// Retrieves a greeting depending on the given game context.
  /// </summary>
  Task<IReadOnlyCollection<DialogueOption>> GetPossibleDialogueOptionsAsync(GameContext gameContext, CancellationToken cancellationToken);

  #endregion
}
