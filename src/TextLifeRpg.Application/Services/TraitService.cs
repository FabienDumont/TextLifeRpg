using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for managing traits.
/// </summary>
public class TraitService(ITraitRepository traitRepository) : ITraitService
{
  #region Implementation of ITraitService

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Trait>> GetAllTraitsAsync(CancellationToken cancellationToken)
  {
    return await traitRepository.GetAllAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<(Trait, Trait)>> GetIncompatibleTraitsAsync(CancellationToken cancellationToken)
  {
    return await traitRepository.GetIncompatibleTraitsAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Trait>> GetCompatibleTraitsAsync(
    IEnumerable<Guid> selectedTraitsIds, CancellationToken cancellationToken
  )
  {
    return await traitRepository.GetCompatibleTraitsAsync(selectedTraitsIds, cancellationToken);
  }

  #endregion
}
