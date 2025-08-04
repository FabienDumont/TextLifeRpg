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
  /// <param name="npc">The NPC character for whom the greeting is to be retrieved.</param>
  /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
  /// <returns>A task representing the asynchronous operation. The task result contains the greeting information associated with the NPC.</returns>
  Task<Greeting> GetGreetingAsync(GameSave gameSave, Character npc, CancellationToken cancellationToken = default);

  #endregion
}
