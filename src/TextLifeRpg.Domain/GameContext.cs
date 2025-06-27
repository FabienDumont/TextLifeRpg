namespace TextLifeRpg.Domain;

/// <summary>
///   Represents the current context of the game for condition evaluation and interaction handling.
///   Includes the acting character, the world state, and an optional target character (e.g., an NPC).
/// </summary>
public class GameContext
{
  #region Properties

  /// <summary>
  ///   The character performing the action.
  /// </summary>
  public required Character Actor { get; set; }

  /// <summary>
  ///   The current state of the game world, including characters, time, and other global data.
  /// </summary>
  public required World World { get; set; }

  /// <summary>
  ///   The character being interacted with, if any (e.g., an NPC in a dialogue).
  ///   Optional.
  /// </summary>
  public Character? Target { get; init; }

  #endregion
}
