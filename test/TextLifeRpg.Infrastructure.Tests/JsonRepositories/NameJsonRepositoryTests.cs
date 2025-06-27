using TextLifeRpg.Infrastructure.JsonRepositories;

namespace TextLifeRpg.Infrastructure.Tests.JsonRepositories;

public class NameJsonRepositoryTests : IDisposable
{
  #region Fields

  private readonly string _tempDir;

  #endregion

  #region Ctors

  public NameJsonRepositoryTests()
  {
    _tempDir = Path.Combine(Path.GetTempPath(), $"NameRepoTest_{Guid.NewGuid()}");
    Directory.CreateDirectory(_tempDir);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetFemaleNamesAsync_ShouldReturnNames_FromJsonFile()
  {
    // Arrange
    var path = Path.Combine(_tempDir, "FemaleNames.json");
    await File.WriteAllTextAsync(path, """ ["Alice", "Beth", "Clara"] """);

    var repo = new NameJsonRepository(_tempDir);

    // Act
    var names = await repo.GetFemaleNamesAsync();

    // Assert
    Assert.NotNull(names);
    Assert.Equal(3, names.Count);
    Assert.Contains("Alice", names);
    Assert.Contains("Beth", names);
    Assert.Contains("Clara", names);
  }

  [Fact]
  public async Task GetMaleNamesAsync_ShouldReturnNames_FromJsonFile()
  {
    // Arrange
    var path = Path.Combine(_tempDir, "MaleNames.json");
    await File.WriteAllTextAsync(path, """ ["John", "Mike", "Steve"] """);

    var repo = new NameJsonRepository(_tempDir);

    // Act
    var names = await repo.GetMaleNamesAsync();

    // Assert
    Assert.NotNull(names);
    Assert.Equal(3, names.Count);
    Assert.Contains("John", names);
    Assert.Contains("Mike", names);
    Assert.Contains("Steve", names);
  }

  #endregion

  #region Implementation of IDisposable

  #region Cleanup

  public void Dispose()
  {
    if (Directory.Exists(_tempDir))
    {
      Directory.Delete(_tempDir, true);
    }
  }

  #endregion

  #endregion
}
