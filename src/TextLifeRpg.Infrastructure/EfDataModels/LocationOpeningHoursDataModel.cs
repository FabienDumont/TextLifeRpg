using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
///   EF Core data model representing a location's opening hours.
/// </summary>
[Table("LocationOpeningHours")]
[PrimaryKey(nameof(Id))]
public class LocationOpeningHoursDataModel
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  ///   Identifier of the location this entry belongs to.
  /// </summary>
  [Column("LocationId", Order = 2)]
  [Required]
  public Guid LocationId { get; set; }

  /// <summary>
  ///   Day of the week the location is open.
  /// </summary>
  [Column("DayOfWeek", Order = 3)]
  [Required]
  public DayOfWeek DayOfWeek { get; set; }

  /// <summary>
  ///   Opening time.
  /// </summary>
  [Column("OpensAt", Order = 4)]
  [Required]
  public TimeSpan OpensAt { get; set; }

  /// <summary>
  ///   Closing time.
  /// </summary>
  [Column("ClosesAt", Order = 5)]
  [Required]
  public TimeSpan ClosesAt { get; set; }

  /// <summary>
  ///   Navigation property to the location.
  /// </summary>
  [ForeignKey(nameof(LocationId))]
  public LocationDataModel? Location { get; set; }

  #endregion
}
