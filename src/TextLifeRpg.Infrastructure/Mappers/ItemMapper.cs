using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="Item" /> domain models and <see cref="ItemDataModel" /> EF data models.
/// </summary>
public static class ItemMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static Item ToDomain(this ItemDataModel dataModel)
  {
    return dataModel.Map(i => Item.Load(i.Id, i.Name));
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<Item> ToDomainCollection(this IEnumerable<ItemDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static ItemDataModel ToDataModel(this Item domain)
  {
    return domain.Map(u => new ItemDataModel
      {
        Id = u.Id,
        Name = u.Name
      }
    );
  }

  #endregion
}
