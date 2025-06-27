using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

public static class TextLineMapper
{
  #region Methods

  /// <summary>
  ///   Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static TextLine ToDomain(this TextLineDataModel dataModel)
  {
    var textParts = dataModel.TextParts.Select(tp => new TextPart(tp.Color, tp.Text)).ToList();

    return new TextLine(textParts);
  }

  /// <summary>
  ///   Maps a collection of JSON data models to domain models.
  /// </summary>
  public static List<TextLine> ToDomainCollection(this IEnumerable<TextLineDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static TextLineDataModel ToDataModel(this TextLine domain)
  {
    var textParts = domain.TextParts.Select(tp => new TextPartDataModel
      {
        Color = tp.Color,
        Text = tp.Text
      }
    ).ToList();

    return new TextLineDataModel {TextParts = textParts};
  }

  /// <summary>
  ///   Maps a collection of domain models to their JSON data model counterparts.
  /// </summary>
  public static List<TextLineDataModel> ToDataModelCollection(this IEnumerable<TextLine> domainModels)
  {
    return domainModels.MapCollection(ToDataModel);
  }

  #endregion
}
