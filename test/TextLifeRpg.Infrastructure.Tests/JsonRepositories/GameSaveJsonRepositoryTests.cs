using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.JsonRepositories;

namespace TextLifeRpg.Infrastructure.Tests.JsonRepositories;

public class GameSaveJsonRepositoryTests : IDisposable
{
  #region Fields

  private readonly string _tempDir;

  #endregion

  #region Ctors

  public GameSaveJsonRepositoryTests()
  {
    _tempDir = Path.Combine(Path.GetTempPath(), "GameSaveTests_" + Guid.NewGuid());

    if (Directory.Exists(_tempDir))
    {
      Directory.Delete(_tempDir, true);
    }

    Directory.CreateDirectory(_tempDir);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task SaveAsync_Then_LoadAsync_Should_Persist_GameSave_Correctly()
  {
    // Arrange
    var repo = new GameSaveJsonRepository(_tempDir);
    var playerCharacter = new CharacterBuilder().Build();
    playerCharacter.AddTraits([Guid.NewGuid(), Guid.NewGuid()]);
    var world = World.Create(DateTime.Now, [playerCharacter]);
    var save = GameSave.Create(playerCharacter, world);

    // Act
    await repo.SaveAsync(save, CancellationToken.None);
    var path = Path.Combine(_tempDir, save.Name + ".json");
    var json = await File.ReadAllTextAsync(path, CancellationToken.None);
    var loaded = repo.Load(json);

    // Assert
    Assert.NotNull(loaded);
    Assert.Equal(save.Id, loaded.Id);
    Assert.Equal(save.Name, loaded.Name);
    Assert.Equal(playerCharacter.Id, loaded.PlayerCharacterId);
    Assert.Equal(playerCharacter.Name, loaded.PlayerCharacter.Name);
    Assert.Equal(playerCharacter.TraitsId.OrderBy(x => x), loaded.PlayerCharacter.TraitsId.OrderBy(x => x));
    Assert.Equal(world.CurrentDate, loaded.World.CurrentDate);
    Assert.Equal(world.Characters.Count, loaded.World.Characters.Count);
  }

  [Fact]
  public void Load_Should_Return_Null_If_Json_Is_Invalid()
  {
    // Arrange
    var repo = new GameSaveJsonRepository();
    var invalidJson = "{ not even close to valid json";

    // Act
    var result = repo.Load(invalidJson);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public void GetSavePath_Should_Return_Path_Ending_With_Saves()
  {
    // Act
    var path = GameSaveJsonRepository.GetSavePath("CharacterName");

    // Assert
    Assert.False(string.IsNullOrWhiteSpace(path));
    Assert.EndsWith(Path.Combine("Saves", "CharacterName"), path);
  }

  [Fact]
  public void GetSavePath_Should_Handle_Electron_Path_Correctly()
  {
    // Arrange
    var fakeExePath = Path.Combine("C:", "mygame", "resources", "app", "server");

    // Act
    var savePath = GameSaveJsonRepository.GetSavePath("CharacterName", fakeExePath);

    // Assert
    var expected = Path.Combine("C:", "mygame", "Saves", "CharacterName");
    Assert.Equal(expected, savePath);
  }

  #endregion

  #region Implementation of IDisposable

  public void Dispose()
  {
    if (Directory.Exists(_tempDir))
    {
      Directory.Delete(_tempDir, true);
    }
  }

  #endregion
}
