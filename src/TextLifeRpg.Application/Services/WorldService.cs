using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for managing worlds.
/// </summary>
public class WorldService(
  ICharacterService characterService, ILocationService locationService, IRoomService roomService,
  IRelationshipService relationshipService, IScheduleService scheduleService
) : IWorldService
{
  #region Implementation of IWorldService

  /// <inheritdoc />
  public async Task<World> CreateNewWorldAsync(
    DateTime date, Character playerCharacter, GameSettings gameSettings, CancellationToken cancellationToken
  )
  {
    var spawnRoom = await roomService.GetPlayerSpawnAsync(cancellationToken);

    if (spawnRoom is null)
    {
      throw new InvalidOperationException("No spawn room found.");
    }

    var spawnLocation = await locationService.GetByIdAsync(spawnRoom.LocationId, cancellationToken);
    playerCharacter.MoveTo(spawnLocation.Id, spawnRoom.Id);

    var world = World.Create(date, [playerCharacter]);

    // Create NPCs
    var npcs = new List<Character>();
    for (var i = 0; i < gameSettings.GetNpcCount(); i++)
    {
      var npc = await characterService.CreateRandomCharacterAsync(DateOnly.FromDateTime(date));
      world.AddCharacter(npc);
      npcs.Add(npc);
    }

    var relationships = relationshipService.GenerateRelationships(npcs, DateOnly.FromDateTime(world.CurrentDate));
    world.AddRelationships(relationships);

    var couples = relationships.Where(r =>
      r.Type is RelationshipType.RomanticPartner or RelationshipType.Spouse && r.SourceCharacterId < r.TargetCharacterId
    ).Select(r =>
      {
        var a = world.Characters.Single(c => c.Id == r.SourceCharacterId);
        var b = world.Characters.Single(c => c.Id == r.TargetCharacterId);
        return (a, b);
      }
    ).ToList();

    var (children, childRelationships) = await relationshipService.GenerateChildrenFromCouplesAsync(
      couples, DateOnly.FromDateTime(world.CurrentDate)
    );

    foreach (var child in children)
    {
      world.AddCharacter(child);
    }

    world.AddRelationships(childRelationships);

    world.SetSchedules(
      await scheduleService.GenerateSchedulesAsync(npcs, cancellationToken)
    );

    RefreshCharactersLocation(world, playerCharacter.Id);

    return world;
  }

  /// <inheritdoc />
  public void AdvanceTime(World world, Guid playerCharacterId, int minutes)
  {
    world.AdvanceTime(minutes);

    RefreshCharactersLocation(world, playerCharacterId);
  }

  #endregion

  #region Methods

  /// <summary>
  /// Refreshes the location of NPCs.
  /// </summary>
  private static void RefreshCharactersLocation(World world, Guid playerCharacterId)
  {
    foreach (var character in world.Characters.Where(c => c.Id != playerCharacterId))
    {
      SetCharacterLocation(world, character);
    }
  }

  /// <summary>
  /// Sets a character location depending on schedule.
  /// </summary>
  private static void SetCharacterLocation(World world, Character character)
  {
    var timeOfDay = world.CurrentDate.TimeOfDay;
    var schedule = world.Schedules.FirstOrDefault(s => s.CharacterId == character.Id);

    if (schedule is null)
    {
      character.MoveTo(null, null);
    }
    else
    {
      var currentEntry = schedule.GetCurrentEntry(world.CurrentDate.DayOfWeek, timeOfDay);
      character.MoveTo(currentEntry?.LocationId, currentEntry?.RoomId);
    }
  }

  #endregion
}
