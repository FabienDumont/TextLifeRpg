namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a greeting.
/// </summary>
public class Greeting
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Text spoken when the greeting is triggered.
  /// </summary>
  public string SpokenText { get; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Greeting(Guid id, string spokenText)
  {
    Id = id;
    SpokenText = spokenText;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Greeting Create(string spokenText)
  {
    return new Greeting(Guid.NewGuid(), spokenText);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static Greeting Load(Guid id, string spokenText)
  {
    return new Greeting(id, spokenText);
  }

  #endregion
}
