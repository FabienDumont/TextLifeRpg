using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Constants;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class CharacterServiceTests
{
  #region Fields

  private readonly CharacterService _characterService;
  private readonly INameRepository _nameRepository = A.Fake<INameRepository>();
  private readonly ITraitService _traitService = A.Fake<ITraitService>();
  private readonly IJobService _jobService = A.Fake<IJobService>();
  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();

  #endregion

  #region Ctors

  public CharacterServiceTests()
  {
    _characterService = new CharacterService(_nameRepository, _traitService, _jobService, _randomProvider);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task CreateRandomCharacter_ShouldReturnValidCharacter()
  {
    // Arrange
    var world = World.Create(new DateTime(2025, 1, 1), []);
    A.CallTo(() => _nameRepository.GetFemaleNamesAsync(A<CancellationToken>._)).Returns(new List<string> {"Alice"});
    A.CallTo(() => _nameRepository.GetMaleNamesAsync(A<CancellationToken>._)).Returns(new List<string> {"Bob"});

    var job = Job.Load(Guid.NewGuid(), JobNames.Janitor, 13, 5);
    A.CallTo(() => _jobService.GetAllJobsAsync(A<CancellationToken>._)).Returns(new List<Job> {job});

    // Act
    var character = await _characterService.CreateRandomCharacterAsync(world, CancellationToken.None);

    // Assert
    Assert.NotNull(character);
    Assert.False(string.IsNullOrWhiteSpace(character.Name));
    Assert.True(character.BiologicalSex is BiologicalSex.Male or BiologicalSex.Female);
    Assert.Equal(job.Id, character.JobId);
  }

  [Fact]
  public async Task CreateChildAsync_ShouldReturnChildWithValidProperties()
  {
    // Arrange

    var mother = new CharacterBuilder().WithName("Mom").WithBirthDate(new DateOnly(1980, 1, 1))
      .WithSex(BiologicalSex.Female).Build();
    var father = new CharacterBuilder().WithName("Dad").WithBirthDate(new DateOnly(1978, 1, 1))
      .WithSex(BiologicalSex.Male).Build();
    var world = World.Create(new DateTime(2025, 1, 1), [mother, father]);

    const BiologicalSex expectedSex = BiologicalSex.Female;
    const string expectedName = "Chloe";
    const int expectedBirthOffset = 20;

    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._))
      .ReturnsLazily((int min, int max) => min == 18 && max == 40 ? expectedBirthOffset : 0);
    A.CallTo(() => _randomProvider.Next(0, 2)).Returns(1); // Female
    A.CallTo(() => _nameRepository.GetFemaleNamesAsync(A<CancellationToken>._))
      .Returns(new List<string> {expectedName});
    A.CallTo(() => _randomProvider.Next(0, 1)).Returns(0); // Pick first name

    // Act
    var child = await _characterService.CreateChildAsync(mother, father, world, CancellationToken.None);

    // Assert
    Assert.NotNull(child);
    Assert.Equal(expectedSex, child.BiologicalSex);
    Assert.Equal(expectedName, child.Name);

    var expectedBirthDate = mother.BirthDate.AddYears(expectedBirthOffset);
    Assert.Equal(expectedBirthDate, child.BirthDate);
  }

  [Fact]
  public void GetAttractionValue_ShouldReturnZero_WhenSameCharacter()
  {
    // Arrange
    var person = new CharacterBuilder().Build();

    // Act
    var result = _characterService.GetAttractionValue(person, person, new DateOnly(2025, 1, 1));

    // Assert
    Assert.Equal(0, result);
  }

  [Fact]
  public void GetAttractionValue_ShouldCalculate_ForOppositeSex()
  {
    // Arrange
    var source = new CharacterBuilder().WithName("John").WithBirthDate(new DateOnly(1978, 1, 1))
      .WithSex(BiologicalSex.Male).Build();
    var target = new CharacterBuilder().WithName("Jane").WithBirthDate(new DateOnly(1978, 1, 1))
      .WithSex(BiologicalSex.Female).Build();

    // First Next(0,100)  → base attraction = 80
    // Second Next(-60,0) → biological sex bonus/penalty = -10
    A.CallTo(() => _randomProvider.Next(A<int>.Ignored, A<int>.Ignored)).ReturnsNextFromSequence(80, -10);

    // Act
    var result = _characterService.GetAttractionValue(source, target, new DateOnly(2025, 1, 1));

    // Assert: ageDiff = 0 ⇒ age penalty = 0
    Assert.Equal(70, result); // 80 + (-10) + 0
  }

  [Fact]
  public void GetAttractionValue_ShouldApplyAgePenalty_ForSameSex()
  {
    // Arrange
    var older = new CharacterBuilder().WithBirthDate(new DateOnly(1990, 1, 1)).WithSex(BiologicalSex.Male).Build();
    var younger = new CharacterBuilder().WithBirthDate(new DateOnly(1995, 1, 1)).WithSex(BiologicalSex.Male).Build();

    // base attraction = 50
    A.CallTo(() => _randomProvider.Next(0, 100)).Returns(50);

    // Act
    var result = _characterService.GetAttractionValue(older, younger, new DateOnly(2025, 1, 1));

    // Assert
    // ageDiff = 5 -> age penalty = -10, biologicalSex = 0
    Assert.Equal(40, result); // 50 - 10
    // and prove we never called the opposite-sex branch
    A.CallTo(() => _randomProvider.Next(-60, 0)).MustNotHaveHappened();
  }

  [Fact]
  public void GenerateTraits_ShouldReturnCompatibleSet()
  {
    // Arrange
    var id1 = Guid.NewGuid();
    var id2 = Guid.NewGuid();
    var id3 = Guid.NewGuid();
    var id4 = Guid.NewGuid();

    var allTraits = new List<Guid> {id1, id2, id3, id4};
    var incompatiblePairs = new List<(Guid A, Guid B)> {(id1, id2)};

    A.CallTo(() => _randomProvider.Next(0, 2)).Returns(0); // always choose first trait
    A.CallTo(() => _randomProvider.Next(0, 3)).Returns(0);
    A.CallTo(() => _randomProvider.Next(0, 1)).Returns(0);

    // Act
    var result = _characterService.GenerateTraits(allTraits, incompatiblePairs, 2, new[] {id1});

    // Assert
    Assert.Contains(id1, result);
    Assert.DoesNotContain(id2, result); // incompatible with id1
    Assert.True(result.Count <= 2);
  }

  [Fact]
  public async Task GenerateTraitIdsAsync_ShouldReturnExpectedTraitIds()
  {
    // Arrange
    var id1 = Guid.NewGuid();
    var id2 = Guid.NewGuid();
    var id3 = Guid.NewGuid();
    var trait1 = Trait.Load(id1, "Trait 1");
    var trait2 = Trait.Load(id2, "Trait 2");
    var trait3 = Trait.Load(id3, "Trait 3");

    A.CallTo(() => _traitService.GetAllTraitsAsync(A<CancellationToken>._)).Returns(
      new List<Trait>
      {
        trait1,
        trait2,
        trait3
      }
    );

    A.CallTo(() => _traitService.GetIncompatibleTraitsAsync(A<CancellationToken>._)).Returns(
      new List<(Trait, Trait)>
      {
        (trait1, trait2)
      }
    );

    A.CallTo(() => _randomProvider.Next(0, 2)).Returns(0);
    A.CallTo(() => _randomProvider.Next(0, 1)).Returns(0);

    // Act
    var result = await _characterService.GenerateTraitIdsAsync(2);

    // Assert
    Assert.True(result.Count <= 2);
    Assert.DoesNotContain(result, id => id == id2 && result.Contains(id1));
  }

  [Fact]
  public async Task CreateChildAsync_ShouldInheritTwoTraitsAtMost()
  {
    // Arrange
    var trait1 = Guid.NewGuid();
    var trait2 = Guid.NewGuid();
    var trait3 = Guid.NewGuid();
    var mother = new CharacterBuilder().WithName("Mom").WithBirthDate(new DateOnly(1980, 1, 1))
      .WithSex(BiologicalSex.Female).Build();
    var father = new CharacterBuilder().WithName("Dad").WithBirthDate(new DateOnly(1980, 1, 1))
      .WithSex(BiologicalSex.Male).Build();
    var world = World.Create(new DateTime(2025, 1, 1), [mother, father]);

    mother.AddTraits(new[] {trait1, trait2});
    father.AddTraits(new[] {trait3});

    var childName = "Kid";
    var allTraits = new List<Trait>
      {Trait.Load(trait1, "Trait 1"), Trait.Load(trait2, "Trait 2"), Trait.Load(trait3, "Trait 3")};

    A.CallTo(() => _nameRepository.GetMaleNamesAsync(A<CancellationToken>._)).Returns(new List<string> {childName});
    A.CallTo(() => _randomProvider.Next(0, 2)).Returns(0); // Male
    A.CallTo(() => _randomProvider.Next(18, A<int>._)).Returns(20); // Age offset
    A.CallTo(() => _randomProvider.Next(0, 1)).Returns(0);
    A.CallTo(() => _randomProvider.NextDouble()).ReturnsNextFromSequence(0.1, 0.5, 0.9); // Shuffles the traits

    A.CallTo(() => _traitService.GetAllTraitsAsync(A<CancellationToken>._)).Returns(allTraits);
    A.CallTo(() => _traitService.GetIncompatibleTraitsAsync(A<CancellationToken>._))
      .Returns(new List<(Trait, Trait)>());

    A.CallTo(() => _randomProvider.Next(1, 4)).Returns(3);

    // Act
    var child = await _characterService.CreateChildAsync(mother, father, world, CancellationToken.None);

    // Assert
    Assert.Equal(childName, child.Name);
    Assert.InRange(child.TraitsId.Count, 1, 3);
    Assert.True(child.TraitsId.All(t => new[] {trait1, trait2, trait3}.Contains(t)));
  }

  [Fact]
  public void GenerateTraits_ShouldSkipIncompatiblePreferredTraits()
  {
    // Arrange
    var id1 = Guid.NewGuid();
    var id2 = Guid.NewGuid();
    var id3 = Guid.NewGuid();

    var allTraits = new List<Guid> {id1, id2, id3};
    var incompatiblePairs = new List<(Guid A, Guid B)> {(id1, id2)}; // id1 incompatible with id2

    var preferred = new List<Guid> {id1, id2}; // id1 gets added, id2 should be skipped (incompatible with id1)

    A.CallTo(() => _randomProvider.Next(0, A<int>._)).Returns(0); // Always pick the first trait from pool

    // Act
    var result = _characterService.GenerateTraits(allTraits, incompatiblePairs, 3, preferred);

    // Assert
    Assert.Contains(id1, result); // was compatible, got added
    Assert.DoesNotContain(id2, result); // should be skipped due to incompatibility with id1
  }

  [Fact]
  public void GenerateTraits_ShouldBreakWhenTargetCountReachedWithPreferredOnly()
  {
    // Arrange
    var id1 = Guid.NewGuid();
    var id2 = Guid.NewGuid();
    var id3 = Guid.NewGuid();

    var allTraits = new List<Guid> {id1, id2, id3};
    var preferred = new List<Guid> {id1, id2}; // both will be added

    // Act
    var result = _characterService.GenerateTraits(allTraits, new List<(Guid, Guid)>(), 2, preferred);

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Contains(id1, result);
    Assert.Contains(id2, result);
  }

  [Fact]
  public async Task CreateRandomCharacter_ShouldNotAssignJob_WhenMaxWorkersReached()
  {
    // Arrange
    var job = Job.Load(Guid.NewGuid(), JobNames.Janitor, 13, 1);
    A.CallTo(() => _jobService.GetAllJobsAsync(A<CancellationToken>._)).Returns(new List<Job> { job });

    var existing = new CharacterBuilder().WithJob(job.Id).Build();
    var world = World.Create(new DateTime(2025, 1, 1), [existing]);

    A.CallTo(() => _nameRepository.GetMaleNamesAsync(A<CancellationToken>._)).Returns(new List<string> { "Bob" });
    A.CallTo(() => _randomProvider.Next(0, 2)).Returns(0);
    A.CallTo(() => _randomProvider.Next(1, 4)).Returns(1);
    A.CallTo(() => _randomProvider.Next(0, 1)).Returns(0);
    A.CallTo(() => _traitService.GetAllTraitsAsync(A<CancellationToken>._)).Returns(new List<Trait>());
    A.CallTo(() => _traitService.GetIncompatibleTraitsAsync(A<CancellationToken>._)).Returns(new List<(Trait, Trait)>());

    // Act
    var character = await _characterService.CreateRandomCharacterAsync(world, CancellationToken.None);

    // Assert
    Assert.Null(character.JobId); // No job assigned due to max workers
  }


  #endregion
}
