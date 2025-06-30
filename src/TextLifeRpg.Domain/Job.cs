namespace TextLifeRpg.Domain;

/// <summary>
/// Represents a job with an identifier, a name, and an hourly income.
/// </summary>
public class Job
{
  #region Properties

  /// <summary>
  /// Gets or sets the unique identifier for the job.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets the name of the job.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Represents the income generated per hour for a job.
  /// </summary>
  public int HourIncome { get; set; }

  /// <summary>
  /// Gets or sets the maximum number of workers that can be assigned to this job.
  /// </summary>
  public int MaxWorkers { get; set; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Job(Guid id, string name, int hourIncome, int maxWorkers)
  {
    Id = id;
    Name = name;
    HourIncome = hourIncome;
    MaxWorkers = maxWorkers;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Job Create(string name, int hourIncome, int maxWorkers)
  {
    return new Job(Guid.NewGuid(), name, hourIncome, maxWorkers);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static Job Load(Guid id, string name, int hourIncome, int maxWorkers)
  {
    return new Job(id, name, hourIncome, maxWorkers);
  }

  #endregion
}
