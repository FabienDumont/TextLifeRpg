using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing an item.
/// </summary>
[Table("Items")]
[PrimaryKey(nameof(Id))]
[Index(nameof(Name), IsUnique = true)]
public class ItemDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Name of the trait.
  /// </summary>
  [Column("Name", Order = 2)]
  [Required]
  [MaxLength(100)]
  public required string Name { get; set; }

  #endregion
}
