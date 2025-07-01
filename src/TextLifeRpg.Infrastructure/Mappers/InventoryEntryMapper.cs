using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="InventoryEntry" /> domain models and <see cref="InventoryEntryDataModel" /> JSON data models.
/// </summary>
public static class InventoryEntryMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static InventoryEntry ToDomain(this InventoryEntryDataModel dataModel)
  {
    return dataModel.Map(i => InventoryEntry.Create(i.ItemId, i.Quantity));
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<InventoryEntry> ToDomainCollection(this IEnumerable<InventoryEntryDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static InventoryEntryDataModel ToDataModel(this InventoryEntry domain)
  {
    return domain.Map(u => new InventoryEntryDataModel
      {
        ItemId = u.ItemId,
        Quantity = u.Quantity
      }
    );
  }

  /// <summary>
  /// Maps a collection of domain models to a collection of JSON data models.
  /// </summary>
  public static List<InventoryEntryDataModel> ToDataModelCollection(this IEnumerable<InventoryEntry> domains)
  {
    return domains.MapCollection(ToDataModel);
  }

  #endregion
}
