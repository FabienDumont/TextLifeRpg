using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for traits.
/// </summary>
public class TraitRepository(ApplicationContext context) : RepositoryBase(context), ITraitRepository
{
  #region Implementation of ITraitRepository

  /// <inheritdoc />
  public async Task<Trait?> GetById(Guid id, CancellationToken cancellationToken)
  {
    var trait = await Context.Traits.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    return trait?.ToDomain();
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Trait>> GetAllAsync(CancellationToken cancellationToken)
  {
    var dataModels = await Context.Traits.ToListAsync(cancellationToken).ConfigureAwait(false);

    return dataModels.ToDomainCollection();
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<(Trait, Trait)>> GetIncompatibleTraitsAsync(CancellationToken cancellationToken)
  {
    var incompatibleTraits = await Context.TraitIncompatibilities.ToListAsync(cancellationToken).ConfigureAwait(false);

    var tuples = new List<(Trait, Trait)>();

    foreach (var incompatibleTrait in incompatibleTraits)
    {
      var trait1 = await GetById(incompatibleTrait.Trait1Id, cancellationToken);
      var trait2 = await GetById(incompatibleTrait.Trait2Id, cancellationToken);
      if (trait1 is not null && trait2 is not null)
      {
        tuples.Add(new ValueTuple<Trait, Trait>(trait1, trait2));
      }
    }

    return tuples;
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Trait>> GetCompatibleTraitsAsync(
    IEnumerable<Guid> selectedTraitsIdsEnumerable, CancellationToken cancellationToken
  )
  {
    var selectedTraitsIds = selectedTraitsIdsEnumerable.ToList();
    if (selectedTraitsIds.Count == 0)
    {
      return await GetAllAsync(cancellationToken);
    }

    var incompatibleIds = await Context.TraitIncompatibilities
      .Where(it => selectedTraitsIds.Contains(it.Trait1Id) || selectedTraitsIds.Contains(it.Trait2Id))
      .Select(it => selectedTraitsIds.Contains(it.Trait1Id) ? it.Trait2Id : it.Trait1Id).Distinct()
      .ToListAsync(cancellationToken);

    var excludedIds = selectedTraitsIds.Concat(incompatibleIds).ToHashSet();

    var compatibleTraits = await Context.Traits.Where(t => !excludedIds.Contains(t.Id)).ToListAsync(cancellationToken);

    return compatibleTraits.ToDomainCollection();
  }

  #endregion
}
