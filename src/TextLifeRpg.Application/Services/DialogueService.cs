using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

public class DialogueService(IGreetingRepository greetingRepository) : IDialogueService
{
  #region Implementation of IDialogueService

  /// <inheritdoc />
  public async Task<Greeting> GetGreetingAsync(GameSave gameSave, Character npc, CancellationToken cancellationToken = default)
  {
    var context = new GameContext
    {
      Actor = npc,
      World = gameSave.World,
      Target = gameSave.PlayerCharacter
    };

    return await greetingRepository.GetAsync(context, cancellationToken);
  }

  #endregion
}
