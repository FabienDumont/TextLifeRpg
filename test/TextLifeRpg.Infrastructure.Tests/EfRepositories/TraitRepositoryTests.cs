using MockQueryable.FakeItEasy;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class TraitRepositoryTests
{
  #region Fields

  private readonly ApplicationContext _context;

  private readonly List<TraitIncompatibilityDataModel> _incompatibilities =
    [new() {Trait1Id = Guid.NewGuid(), Trait2Id = Guid.NewGuid()}];

  private readonly TraitRepository _repository;

  private readonly List<TraitDataModel> _traitDataModels =
  [
    new() {Id = Guid.NewGuid(), Name = "Brave"},
    new() {Id = Guid.NewGuid(), Name = "Shy"},
    new() {Id = Guid.NewGuid(), Name = "Aggressive"}
  ];

  #endregion

  #region Ctors

  public TraitRepositoryTests()
  {
    _context = A.Fake<ApplicationContext>();

    var traitDbSet = _traitDataModels.BuildMockDbSet();
    var incompatibleDbSet = _incompatibilities.BuildMockDbSet();

    A.CallTo(() => _context.Traits).Returns(traitDbSet);
    A.CallTo(() => _context.TraitIncompatibilities).Returns(incompatibleDbSet);
    A.CallTo(() => _context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new TraitRepository(_context);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetAllAsync_ShouldReturnMappedTraits()
  {
    var result = await _repository.GetAllAsync(CancellationToken.None);

    Assert.NotNull(result);
    Assert.Equal(_traitDataModels.Count, result.Count);
    foreach (var trait in _traitDataModels)
    {
      Assert.Contains(result, r => r.Id == trait.Id);
    }
  }

  [Fact]
  public async Task GetCompatibleTraitsAsync_ShouldReturnAll_WhenNoSelectedTraits()
  {
    var result = await _repository.GetCompatibleTraitsAsync([], CancellationToken.None);

    Assert.NotNull(result);
    Assert.Equal(_traitDataModels.Count, result.Count);
  }

  [Fact]
  public async Task GetCompatibleTraitsAsync_ShouldExcludeIncompatible()
  {
    var traitA = _traitDataModels[0];
    var traitB = _traitDataModels[1];

    _incompatibilities.Clear();
    _incompatibilities.Add(
      new TraitIncompatibilityDataModel
      {
        Trait1Id = traitA.Id,
        Trait2Id = traitB.Id
      }
    );

    var newTraitDb = _traitDataModels.BuildMockDbSet();
    var newIncompatibleDb = _incompatibilities.BuildMockDbSet();

    A.CallTo(() => _context.Traits).Returns(newTraitDb);
    A.CallTo(() => _context.TraitIncompatibilities).Returns(newIncompatibleDb);

    var result = await _repository.GetCompatibleTraitsAsync([traitA.Id], CancellationToken.None);

    Assert.NotNull(result);
    Assert.DoesNotContain(result, r => r.Id == traitA.Id);
    Assert.DoesNotContain(result, r => r.Id == traitB.Id);
  }

  [Fact]
  public async Task GetById_ShouldReturnMappedTrait_WhenItExists()
  {
    // Arrange
    var existing = _traitDataModels[1];

    // Act
    var result = await _repository.GetById(existing.Id, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(existing.Id, result.Id);
    Assert.Equal(existing.Name, result.Name);
  }

  [Fact]
  public async Task GetIncompatibleTraitsAsync_ShouldReturnMappedTuples()
  {
    // Arrange
    _incompatibilities.Clear();
    var t1 = _traitDataModels[0];
    var t2 = _traitDataModels[2];
    _incompatibilities.Add(
      new TraitIncompatibilityDataModel
      {
        Trait1Id = t1.Id,
        Trait2Id = t2.Id
      }
    );

    // Refresh fake DbSets to include new data
    A.CallTo(() => _context.Traits).Returns(_traitDataModels.BuildMockDbSet());
    A.CallTo(() => _context.TraitIncompatibilities).Returns(_incompatibilities.BuildMockDbSet());

    // Act
    var result = await _repository.GetIncompatibleTraitsAsync(CancellationToken.None);

    // Assert
    Assert.Single(result);
    var (a, b) = result.First();
    Assert.True((a.Id == t1.Id && b.Id == t2.Id) || (a.Id == t2.Id && b.Id == t1.Id));
  }

  #endregion
}
