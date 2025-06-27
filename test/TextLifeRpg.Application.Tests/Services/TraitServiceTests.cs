using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Services;

public class TraitServiceTests
{
  #region Fields

  private readonly ITraitRepository _traitRepository = A.Fake<ITraitRepository>();
  private readonly TraitService _traitService;

  #endregion

  #region Ctors

  public TraitServiceTests()
  {
    _traitService = new TraitService(_traitRepository);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetAllTraitsAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var traits = new List<Trait> {Trait.Load(Guid.NewGuid(), "Kind")};
    A.CallTo(() => _traitRepository.GetAllAsync(A<CancellationToken>._)).Returns(traits);

    // Act
    var result = await _traitService.GetAllTraitsAsync(CancellationToken.None);
    var resultList = result.ToList();

    // Assert
    Assert.Equal(traits.Count, resultList.Count);
    Assert.Equal(traits[0].Id, resultList[0].Id);
  }

  [Fact]
  public async Task GetIncompatibleTraitsAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var incompatibleTraits = new List<(Trait, Trait)>
      {(Trait.Load(Guid.NewGuid(), "Kind"), Trait.Load(Guid.NewGuid(), "Mean"))};
    A.CallTo(() => _traitRepository.GetIncompatibleTraitsAsync(A<CancellationToken>._)).Returns(incompatibleTraits);

    // Act
    var result = await _traitService.GetIncompatibleTraitsAsync(CancellationToken.None);
    var resultList = result.ToList();

    // Assert
    Assert.Equal(incompatibleTraits.Count, resultList.Count);
    Assert.Equal(incompatibleTraits[0].Item1.Id, resultList[0].Item1.Id);
  }

  [Fact]
  public async Task GetCompatibleTraitsAsync_ShouldCallRepositoryAndReturnResult()
  {
    // Arrange
    var ids = new[] {Guid.NewGuid()};
    var traits = new List<Trait> {Trait.Load(Guid.NewGuid(), "Polite")};
    A.CallTo(() => _traitRepository.GetCompatibleTraitsAsync(ids, A<CancellationToken>._)).Returns(traits);

    // Act
    var result = await _traitService.GetCompatibleTraitsAsync(ids, CancellationToken.None);
    var resultList = result.ToList();

    // Assert
    Assert.Equal(traits.Count, resultList.Count);
    Assert.Equal(traits[0].Id, resultList[0].Id);
  }

  #endregion
}
