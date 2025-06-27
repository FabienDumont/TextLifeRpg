namespace TextLifeRpg.Blazor.ViewModels;

/// <summary>
/// View model representing a trait used in the UI.
/// </summary>
public class TraitViewModel
{
  #region Properties

  /// <summary>
  /// Unique identifier of the trait.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Display name of the trait.
  /// </summary>
  public string Name { get; init; } = "";

  /// <summary>
  /// Indicates whether the trait is currently selected.
  /// </summary>
  public bool IsSelected { get; set; }

  /// <summary>
  /// Indicates whether the trait is enabled for selection.
  /// </summary>
  public bool IsEnabled { get; set; } = true;

  #endregion
}
