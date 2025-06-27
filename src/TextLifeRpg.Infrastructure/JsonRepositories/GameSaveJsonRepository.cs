using System.Text.Json;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.JsonRepositories;

/// <summary>
/// Repository for saving and loading game data using JSON files.
/// </summary>
public class GameSaveJsonRepository(string? customPath = null) : IGameSaveRepository
{
  #region Methods

  /// <summary>
  /// Determines the file system path where the save file should be stored.
  /// </summary>
  /// <param name="characterName">Name of the character the save is associated with.</param>
  /// <param name="exePathOverride">Optional override for the base directory path.</param>
  /// <returns>Full path to the character's save directory.</returns>
  internal static string GetSavePath(string characterName, string? exePathOverride = null)
  {
    var exePath = exePathOverride ?? AppContext.BaseDirectory;

    if (exePath.Contains(Path.Combine("resources", "app")))
    {
      var parent = Path.GetFullPath(Path.Combine(exePath, @"..\..\.."));

      return Path.Combine(parent, $@"Saves\{characterName}");
    }

    return Path.Combine(exePath, $@"Saves\{characterName}");
  }

  #endregion

  #region Implementation of IGameSaveRepository

  /// <inheritdoc />
  public async Task SaveAsync(GameSave save, CancellationToken cancellationToken)
  {
    var saveDirectory = customPath ?? GetSavePath(save.PlayerCharacter.Name);
    Directory.CreateDirectory(saveDirectory);
    var path = Path.Combine(saveDirectory, $"{save.Name}.json");

    var dataModel = save.ToDataModel();

    var json = JsonSerializer.Serialize(dataModel, new JsonSerializerOptions {WriteIndented = true});
    await File.WriteAllTextAsync(path, json, cancellationToken);
  }

  /// <inheritdoc />
  public GameSave? Load(string json)
  {
    try
    {
      var dataModel = JsonSerializer.Deserialize<GameSaveDataModel>(json);

      return dataModel?.ToDomain();
    }
    catch (JsonException)
    {
      return null;
    }
  }

  #endregion
}
