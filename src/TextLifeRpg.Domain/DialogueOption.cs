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

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private DialogueOption(Guid id, string label)
  {
    Id = id;
    Label = label;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static DialogueOption Create(string label)
  {
    return new DialogueOption(Guid.NewGuid(), label);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static DialogueOption Load(Guid id, string label)
  {
    return new DialogueOption(id, label);
  }

  #endregion
}
