using TextLifeRpg.Domain;

namespace TextLifeRpg.Blazor.Stores;

/// <summary>
///   Stores and manages the current game save state for the UI layer.
/// </summary>
public class GameSaveStore
{
  #region Properties

  /// <summary>
  ///   The currently loaded game save.
  /// </summary>
  public GameSave? CurrentSave { get; private set; }

  #endregion

  #region Methods

  /// <summary>
  ///   Event triggered when the game state changes.
  /// </summary>
  public event Func<Task>? OnAsyncChange;

  /// <summary>
  ///   Loads the given game save and notifies listeners.
  /// </summary>
  public async Task LoadGameAsync(GameSave save)
  {
    CurrentSave = save;
    await NotifyStateChangedAsync();
  }

  /// <summary>
  ///   Triggers the <see cref="OnAsyncChange" /> event to notify subscribers.
  /// </summary>
  public async Task NotifyStateChangedAsync()
  {
    if (OnAsyncChange is not null)
    {
      var invocationList = OnAsyncChange.GetInvocationList().Cast<Func<Task>>();
      foreach (var handler in invocationList)
      {
        try
        {
          await handler();
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine($"Error notifying change: {ex}");
        }
      }
    }
  }

  #endregion
}
