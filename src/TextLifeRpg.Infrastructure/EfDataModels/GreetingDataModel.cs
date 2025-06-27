using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a greeting entry.
/// </summary>
[Table("Greetings")]
[PrimaryKey(nameof(Id))]
public class GreetingDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// The spoken text of the greeting.
  /// </summary>
  [Column("SpokenText", Order = 2)]
  [Required]
  [MaxLength(1000)]
  public required string SpokenText { get; set; }

  #endregion
}
