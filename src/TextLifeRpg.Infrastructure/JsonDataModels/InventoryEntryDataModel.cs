namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
/// JSON data model representing an inventory entry for serialization.
/// </summary>
public class InventoryEntryDataModel
{
  #region Properties

  public Guid ItemId { get; init; }
  public int Quantity { get; init; }

  #endregion
}
