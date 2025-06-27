using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
///   EF Core data model representing a condition.
/// </summary>
[Table("Conditions")]
[PrimaryKey(nameof(Id))]
public class ConditionDataModel
{
  #region Properties

  [Required]
  public Guid Id { get; set; }

  [Required]
  public required ContextType ContextType { get; set; }

  [Required]
  public Guid ContextId { get; set; }

  [Required]
  public required ConditionType ConditionType { get; set; }

  [MaxLength(10)]
  public string? OperandLeft { get; set; }

  [Required]
  [MaxLength(100)]
  public required string Operator { get; set; }

  [MaxLength(10)]
  public string? OperandRight { get; set; }

  public bool Negate { get; set; }

  #endregion
}

public enum ContextType
{
  Greeting = 0,
  ExplorationActionResult = 1,
  ExplorationActionResultNarration = 2,
  Narration = 3
}

public enum ConditionType
{
  ActorHasTrait = 0,
  ActorEnergy = 1,
  ActorRelationship = 1,
  ActorMoney
}
