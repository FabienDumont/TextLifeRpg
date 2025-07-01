namespace TextLifeRpg.Domain;

/// <summary>
/// Represents an entry in the inventory, containing details of a specific item and its quantity.
/// </summary>
public class InventoryEntry
{
  #region Properties

  /// <summary>
  /// Gets the unique identifier of the item associated with this inventory entry.
  /// </summary>
  public Guid ItemId { get; }

  /// <summary>
  /// Represents the quantity of an item in the inventory.
  /// Determines the number of units of a specific item contained within an inventory entry.
  /// </summary>
  public int Quantity { get; private set; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private InventoryEntry(Guid itemId, int quantity)
  {
    ItemId = itemId;
    Quantity = quantity;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static InventoryEntry Create(Guid itemId, int quantity)
  {
    return new InventoryEntry(itemId, quantity);
  }

  /// <summary>
  /// Adds a specified amount to the quantity of the inventory entry.
  /// </summary>
  /// <param name="amount">The amount to be added. Must be greater than zero.</param>
  public void Add(int amount)
  {
    Quantity += amount;
  }

  /// <summary>
  /// Removes a specified amount from the current quantity.
  /// If the amount to remove is less than or equal to zero, the operation is ignored.
  /// If the amount to remove exceeds the current quantity, the quantity is set to zero.
  /// </summary>
  /// <param name="amount">The number of items to be removed. Must be a positive integer.</param>
  public void Remove(int amount)
  {
    Quantity = Math.Max(0, Quantity - amount);
  }

  #endregion
}
