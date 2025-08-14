namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a dialogue option.
/// </summary>
public class DialogueOption
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Label describing the dialogue option.
  /// </summary>
  public string Label { get; }

  /// <summary>
  /// Amount of time, in minutes, required for the dialogue option.
  /// </summary>
  public int NeededMinutes { get; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private DialogueOption(Guid id, string label, int neededMinutes)
  {
    Id = id;
    Label = label;
    NeededMinutes = neededMinutes;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static DialogueOption Create(string label, int neededMinutes)
  {
    return new DialogueOption(Guid.NewGuid(), label, neededMinutes);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static DialogueOption Load(Guid id, string label, int neededMinutes)
  {
    return new DialogueOption(id, label, neededMinutes);
  }

  #endregion
}
