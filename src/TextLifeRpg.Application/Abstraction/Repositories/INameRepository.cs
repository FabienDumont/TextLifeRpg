namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
///   Repository interface for retrieving character names.
/// </summary>
public interface INameRepository
{
  #region Methods

  /// <summary>
  ///   Retrieves a list of female character names.
  /// </summary>
  Task<IReadOnlyList<string>> GetFemaleNamesAsync(CancellationToken cancellationToken = default);

  /// <summary>
  ///   Retrieves a list of male character names.
  /// </summary>
  Task<IReadOnlyList<string>> GetMaleNamesAsync(CancellationToken cancellationToken = default);

  #endregion
}
