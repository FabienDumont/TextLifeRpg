using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
///   EF Core data model representing a pair of incompatible traits.
/// </summary>
[Table("TraitIncompatibilities")]
[PrimaryKey(nameof(Trait1Id), nameof(Trait2Id))]
public class TraitIncompatibilityDataModel
{
  #region Properties

  /// <summary>
  ///   Identifier of the first trait.
  /// </summary>
  [Column("Trait1Id", Order = 1)]
  [Required]
  public Guid Trait1Id { get; set; }

  /// <summary>
  ///   Identifier of the second trait.
  /// </summary>
  [Column("Trait2Id", Order = 2)]
  [Required]
  public Guid Trait2Id { get; set; }

  /// <summary>
  ///   Navigation to the first trait.
  /// </summary>
  [ForeignKey(nameof(Trait1Id))]
  public TraitDataModel? Trait1 { get; set; }

  /// <summary>
  ///   Navigation to the second trait.
  /// </summary>
  [ForeignKey(nameof(Trait2Id))]
  public TraitDataModel? Trait2 { get; set; }

  #endregion
}
