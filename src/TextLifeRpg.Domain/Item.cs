namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing an item in the game.
/// </summary>
public class Item
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public string Name { get; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Item(Guid id, string name)
  {
    Id = id;
    Name = name;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Item Create(string name)
  {
    return new Item(Guid.NewGuid(), name);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static Item Load(Guid id, string name)
  {
    return new Item(id, name);
  }

  #endregion
}
