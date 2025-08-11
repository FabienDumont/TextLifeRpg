using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for managing characters.
/// </summary>
public class CharacterService(
  INameRepository nameRepository, ITraitService traitService, IJobService jobService, IRandomProvider randomProvider
) : ICharacterService
{
  #region Fields

  private IReadOnlyCollection<Trait>? _cachedTraits;
  private IReadOnlyList<(Guid A, Guid B)>? _cachedIncompatibleTraits;
  private IReadOnlyCollection<Job>? _cachedJobs;

  #endregion

  #region Implementation of ICharacterService

  /// <inheritdoc />
  public async Task<Character> CreateRandomCharacterAsync(World world, CancellationToken cancellationToken)
  {
    const int minAge = 18, maxAge = 69;
    var age = randomProvider.Next(minAge, maxAge + 1);
    var birthDate = DateOnly.FromDateTime(world.CurrentDate.AddYears(-age).AddDays(randomProvider.Next(0, 365)));
    var sex = randomProvider.Next(0, 2) == 0 ? BiologicalSex.Male : BiologicalSex.Female;

    return await CreateCharacterAsync(
      world, birthDate, sex, randomProvider.Next(1, 4), cancellationToken: cancellationToken
    );
  }

  /// <inheritdoc />
  public int GetAttractionValue(Character source, Character target, DateOnly gameDate)
  {
    // Can't be attracted to self
    if (source.Id == target.Id)
    {
      return 0;
    }

    var baseAttraction = randomProvider.Next(0, 100);

    var biologicalSex = source.BiologicalSex != target.BiologicalSex ? randomProvider.Next(-60, 0) : 0;

    // Age difference penalty (linear)
    var ageDiff = Math.Abs(source.GetAge(gameDate) - target.GetAge(gameDate));
    var age = -(ageDiff * 2);

    // Final score
    var result = baseAttraction + biologicalSex + age;

    // Clamp between -100 and 100
    return Math.Clamp(result, -100, 100);
  }

  /// <inheritdoc />
  public async Task<Character> CreateChildAsync(
    Character mother, Character father, World world, CancellationToken cancellationToken
  )
  {
    const int motherMinAge = 18;
    var motherMaxAge = Math.Min(39, DateOnly.FromDateTime(world.CurrentDate).Year - mother.BirthDate.Year);
    var motherAgeAtBirth = randomProvider.Next(motherMinAge, motherMaxAge + 1);
    var birthDate = mother.BirthDate.AddYears(motherAgeAtBirth);
    var sex = randomProvider.Next(0, 2) == 0 ? BiologicalSex.Male : BiologicalSex.Female;
    var inherited = mother.TraitsId.Concat(father.TraitsId).OrderBy(_ => randomProvider.NextDouble()).Take(2);

    return await CreateCharacterAsync(world, birthDate, sex, randomProvider.Next(1, 4), inherited, cancellationToken);
  }

  #endregion

  #region Methods

  /// <summary>
  /// Generates 1–3 random, mutually compatible trait IDs.
  /// </summary>
  public List<Guid> GenerateTraits(
    IReadOnlyCollection<Guid> allTraitsIds, IReadOnlyCollection<(Guid A, Guid B)> incompatibleTraitsIds,
    int targetCount, IReadOnlyCollection<Guid>? preferredTraitsIds = null
  )
  {
    var selected = new List<Guid>();

    // Seed with preferred traits (e.g. inherited from parents)
    if (preferredTraitsIds is {Count: > 0})
    {
      foreach (var preferredTraitId in preferredTraitsIds)
      {
        if (!IsCompatible(preferredTraitId))
        {
          continue;
        }

        selected.Add(preferredTraitId);
        if (selected.Count == targetCount) break;
      }
    }

    while (selected.Count < targetCount)
    {
      var pool = allTraitsIds.Where(t => !selected.Contains(t) && IsCompatible(t)).ToList();
      if (pool.Count == 0) break; // No more compatible options

      var choice = pool.ElementAt(randomProvider.Next(0, pool.Count));
      selected.Add(choice);
    }

    return selected;

    bool IsCompatible(Guid id)
    {
      return selected.All(existing =>
        !incompatibleTraitsIds.Contains((existing, id)) && !incompatibleTraitsIds.Contains((id, existing))
      );
    }
  }

  /// <summary>
  /// Generates a list of trait IDs based on the specified target count,
  /// using cached traits and their incompatibility constraints.
  /// </summary>
  /// <param name="targetCount">The number of trait IDs to generate.</param>
  /// <param name="preferred">
  /// An optional collection of preferred trait IDs that can be selected.
  /// </param>
  /// <param name="cancellationToken">A cancellation token used to cancel the operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the generated list of trait IDs.</returns>
  public async Task<List<Guid>> GenerateTraitIdsAsync(
    int targetCount, IReadOnlyCollection<Guid>? preferred = null, CancellationToken cancellationToken = default
  )
  {
    _cachedTraits ??= await traitService.GetAllTraitsAsync(cancellationToken);
    _cachedIncompatibleTraits ??= (await traitService.GetIncompatibleTraitsAsync(cancellationToken))
      .Select(p => (p.Item1.Id, p.Item2.Id)).ToList();

    return GenerateTraits(_cachedTraits.Select(t => t.Id).ToList(), _cachedIncompatibleTraits, targetCount, preferred);
  }

  /// <summary>
  /// Creates a new character with specified attributes and traits.
  /// </summary>
  /// <param name="world">The world.</param>
  /// <param name="birthDate">The birth date of the character.</param>
  /// <param name="sex">The biological sex of the character (Male or Female).</param>
  /// <param name="traitCount">The number of traits to be assigned to the character.</param>
  /// <param name="preferredTraits">An optional collection of preferred trait IDs.</param>
  /// <param name="cancellationToken">A cancellation token used to cancel the operation.</param>
  /// <returns>A newly created <see cref="Character"/> instance populated with the provided parameters and randomly generated values.</returns>
  private async Task<Character> CreateCharacterAsync(
    World world, DateOnly birthDate, BiologicalSex sex, int traitCount, IEnumerable<Guid>? preferredTraits = null,
    CancellationToken cancellationToken = default
  )
  {
    var names = sex == BiologicalSex.Female
      ? await nameRepository.GetFemaleNamesAsync(cancellationToken)
      : await nameRepository.GetMaleNamesAsync(cancellationToken);

    var name = names[randomProvider.Next(0, names.Count)];
    var height = randomProvider.NextClampedHeight(sex);
    var weight = randomProvider.NextClampedWeight(sex, height);
    var muscleMass = randomProvider.NextClampedMuscleMass(sex, height);

    var intelligence = randomProvider.Next(1, 11);
    var charisma = randomProvider.Next(1, 11);

    // Strength based on muscle mass (normalize 15–40kg to 1–10 scale)
    var normalized = (muscleMass - 15) / 25.0; // 0.0 to 1.0
    var baseStrength = (int) (normalized * 9) + 1;

    // Add slight randomness ±1
    var strength = Math.Clamp(baseStrength + randomProvider.Next(-1, 2), 1, 10);

    var attributes = CharacterAttributes.Create(intelligence, strength, charisma);

    var character = Character.Create(name, birthDate, sex, height, weight, muscleMass, attributes);

    var traitIds = await GenerateTraitIdsAsync(traitCount, preferredTraits?.ToList(), cancellationToken);
    character.AddTraits(traitIds);

    var jobs = await GetJobsAsync(cancellationToken);
    var availableJob = jobs.OrderBy(_ => randomProvider.NextDouble())
      .FirstOrDefault(j => world.Characters.Count(c => c.JobId == j.Id) < j.MaxWorkers);

    if (availableJob is not null)
    {
      character.JobId = availableJob.Id;
    }

    return character;
  }

  private async Task<IReadOnlyCollection<Job>> GetJobsAsync(CancellationToken ct = default)
  {
    return _cachedJobs ??= await jobService.GetAllJobsAsync(ct);
  }

  #endregion
}
