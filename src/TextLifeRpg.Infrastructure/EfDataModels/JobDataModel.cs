using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a job.
/// </summary>
[Table("Jobs")]
[PrimaryKey(nameof(Id))]
[Index(nameof(Name), IsUnique = true)]
public class JobDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Name of the job.
  /// </summary>
  [Column("Name", Order = 2)]
  [Required]
  [MaxLength(100)]
  public required string Name { get; set; }

  /// <summary>
  /// Hour income of the job.
  /// </summary>
  [Column("HourIncome", Order = 3)]
  [Required]
  public required int HourIncome { get; set; }

  /// <summary>
  /// Maximum number of workers allowed for the job.
  /// </summary>
  [Column("MaxWorkers", Order = 4)]
  [Required]
  public required int MaxWorkers { get; set; }

  #endregion
}
