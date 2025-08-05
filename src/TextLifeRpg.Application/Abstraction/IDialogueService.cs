using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Represents a service responsible for managing dialogues between characters in the game.
/// </summary>
public interface IDialogueService
{
  #region Methods

  /// <summary>
  /// Asynchronously executes a greeting interaction between the player character and the currently interacting NPC,
  /// updating the game state with the resulting dialogue.
  /// </summary>
  /// <param name="gameSave">The current game save containing the player's state, interacting NPC, and the world context.</param>
  /// <param name="cancellationToken">An optional token to monitor for cancellation requests during execution.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  Task ExecuteGreetingAsync(GameSave gameSave, CancellationToken cancellationToken = default);

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
