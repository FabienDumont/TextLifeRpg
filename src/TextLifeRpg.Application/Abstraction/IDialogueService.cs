using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Represents a service responsible for managing dialogues between characters in the game.
/// </summary>
public interface IDialogueService
{
  #region Methods

  /// <summary>
  /// Asynchronously retrieves a greeting for a specified NPC character based on the current game state.
  /// </summary>
  /// <param name="gameSave">The current game save containing the state of the game world and player information.</param>
  /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
  /// <returns>A task representing the asynchronous operation. The task result contains the greeting information associated with the NPC.</returns>
  Task<Greeting> GetGreetingAsync(GameSave gameSave, CancellationToken cancellationToken = default);

  /// <summary>
  /// Asynchronously retrieves a list of possible dialogue options based on the current game state.
  /// </summary>
  /// <param name="gameSave">The current game save containing the state of the game world and player information.</param>
  /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
  /// <returns>A task representing the asynchronous operation. The task result contains a list of dialogue options available for the player.</returns>
  Task<IReadOnlyCollection<DialogueOption>> GetPossibleDialogueOptionsAsync(
    GameSave gameSave, CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Asynchronously executes a specified dialogue option within the context of the current game state.
  /// </summary>
  /// <param name="dialogueOption">The dialogue option to be executed.</param>
  /// <param name="gameSave">The current game save.</param>
  /// <param name="cancellationToken">An optional cancellation token to monitor for cancellation requests.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  Task ExecuteDialogueOptionAsync(
    DialogueOption dialogueOption, GameSave gameSave, CancellationToken cancellationToken
  );

  #endregion
}
