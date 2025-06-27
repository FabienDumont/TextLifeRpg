namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a character trait.
/// </summary>
public class Trait
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Name of the trait.
  /// </summary>
  public string Name { get; set; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Trait(Guid id, string name)
  {
    Id = id;
    Name = name;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Trait Create(string name)
  {
    return new Trait(Guid.NewGuid(), name);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static Trait Load(Guid id, string name)
  {
    return new Trait(id, name);
  }

  #endregion
}
