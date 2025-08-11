using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for managing saves.
/// </summary>
public class SaveService(
  IGameSaveRepository saveRepository, IWorldService worldService, INarrationService narrationService
) : ISaveService
{
  #region Implementation of ISaveService

  /// <inheritdoc />
  public async Task SaveGameAsync(GameSave save, CancellationToken ct = default)
  {
    await saveRepository.SaveAsync(save, ct);
  }

  /// <inheritdoc />
  public GameSave? LoadGame(string json)
  {
    return saveRepository.Load(json);
  }

  /// <inheritdoc />
  public async Task<GameSave> CreateNewSaveAsync(
    DateTime date, Character playerCharacter, GameSettings gameSettings, CancellationToken cancellationToken
  )
  {
    var world = await worldService.CreateNewWorldAsync(date, playerCharacter, gameSettings, cancellationToken);

    var save = GameSave.Create(playerCharacter, world);

    var narrationText = await narrationService.GetNarrationTextByKeyAsync(
      "GameIntro", playerCharacter, world, cancellationToken
    );

    TextLineBuilder.BuildNarrationLine(narrationText, save.PlayerCharacter, null, save);

    return save;
  }

  #endregion
}
