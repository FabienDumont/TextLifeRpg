namespace TextLifeRpg.Domain;

/// <summary>
///   Domain class representing a narration.
/// </summary>
public class Narration
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  ///   Narration key.
  /// </summary>
  public string Key { get; }

  /// <summary>
  ///   Narration text template.
  /// </summary>
  public string Text { get; }

  #endregion

  #region Ctors

  /// <summary>
  ///   Private constructor used internally.
  /// </summary>
  private Narration(Guid id, string key, string text)
  {
    Id = id;
    Key = key;
    Text = text;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Factory method to create a new instance.
  /// </summary>
  public static Narration Create(string key, string text)
  {
    return new Narration(Guid.NewGuid(), key, text);
  }

  /// <summary>
  ///   Factory method to load an existing instance from persistence.
  /// </summary>
  public static Narration Load(Guid id, string key, string text)
  {
    return new Narration(id, key, text);
  }

  #endregion
}
