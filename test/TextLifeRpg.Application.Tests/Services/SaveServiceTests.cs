using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class SaveServiceTests
{
  #region Fields

  private readonly INarrationService _narrationService = A.Fake<INarrationService>();
  private readonly IGameSaveRepository _repository = A.Fake<IGameSaveRepository>();
  private readonly SaveService _saveService;
  private readonly IWorldService _worldService = A.Fake<IWorldService>();

  #endregion

  #region Ctors

  public SaveServiceTests()
  {
    _saveService = new SaveService(_repository, _worldService, _narrationService);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task SaveGameAsync_Should_Call_Repository_With_Correct_Arguments()
  {
    var playerCharacter = new CharacterBuilder().Build();
    var save = GameSave.Create(playerCharacter, World.Create(DateTime.Now, [playerCharacter]));

    await _saveService.SaveGameAsync(save);

    A.CallTo(() => _repository.SaveAsync(save, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public void LoadGame_Should_Return_GameSave_From_Repository()
  {
    var json = "{ valid json }";
    var playerCharacter = new CharacterBuilder().Build();
    var expectedSave = GameSave.Create(playerCharacter, World.Create(DateTime.Now, [playerCharacter]));

    A.CallTo(() => _repository.Load(json)).Returns(expectedSave);

    var result = _saveService.LoadGame(json);

    Assert.Same(expectedSave, result);
    A.CallTo(() => _repository.Load(json)).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public void LoadGame_Should_Return_Null_When_Repo_Returns_Null()
  {
    A.CallTo(() => _repository.Load("NotJson")).Returns(null);

    var result = _saveService.LoadGame("NotJson");

    Assert.Null(result);
    A.CallTo(() => _repository.Load("NotJson")).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public async Task CreateNewSave_ShouldCreateNewSave_WithCorrectValues()
  {
    var date = new DateTime(2025, 4, 24, 14, 30, 0);
    var playerCharacter = new CharacterBuilder().Build();
    var world = World.Create(date, [playerCharacter]);
    var gameSettings = GameSettings.Create(NpcDensity.Low);

    A.CallTo(() => _worldService.CreateNewWorldAsync(date, playerCharacter, gameSettings, CancellationToken.None))
      .Returns(world);

    var save = await _saveService.CreateNewSaveAsync(date, playerCharacter, gameSettings, CancellationToken.None);

    Assert.NotNull(save);
    Assert.Same(playerCharacter, save.PlayerCharacter);
    Assert.Same(world, save.World);
    Assert.Equal($"{playerCharacter.Name}_{world.CurrentDate:yyyyMMdd_HHmmss}", save.Name);

    A.CallTo(() => _worldService.CreateNewWorldAsync(date, playerCharacter, gameSettings, CancellationToken.None))
      .MustHaveHappenedOnceExactly();
  }

  #endregion
}
