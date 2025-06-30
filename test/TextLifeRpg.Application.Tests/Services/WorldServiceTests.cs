using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class WorldServiceTests
{
  #region Fields

  private readonly ICharacterService _characterService;
  private readonly ILocationService _locationService;
  private readonly IRelationshipService _relationshipService;
  private readonly IScheduleService _scheduleService = A.Fake<IScheduleService>();
  private readonly IRoomService _roomService;

  private readonly WorldService _worldService;

  #endregion

  #region Ctors

  public WorldServiceTests()
  {
    _characterService = A.Fake<ICharacterService>();
    _locationService = A.Fake<ILocationService>();
    _roomService = A.Fake<IRoomService>();
    _relationshipService = A.Fake<IRelationshipService>();
    _worldService = new WorldService(
      _characterService, _locationService, _roomService, _relationshipService, _scheduleService
    );
  }

  #endregion

  #region Methods

  [Fact]
  public async Task CreateNewWorld_ShouldIncludePlayerCharacter_AndAddTenRandomCharacters()
  {
    // Arrange
    var date = new DateTime(2025, 4, 24);
    var playerCharacter = new CharacterBuilder().Build();
    var randomCharacters = new List<Character>();
    var gameSettings = GameSettings.Create(NpcDensity.Low);
    var location = Location.Create("Home");
    var room = Room.Create(location.Id, "Living room", true);

    A.CallTo(() => _roomService.GetPlayerSpawnAsync(CancellationToken.None)).Returns(room);
    A.CallTo(() => _locationService.GetByIdAsync(location.Id, CancellationToken.None)).Returns(location);

    for (var i = 0; i < gameSettings.GetNpcCount(); i++)
    {
      var randomCharacter = new CharacterBuilder().Build();
      randomCharacters.Add(randomCharacter);
    }

    var generatedRelationships = new List<Relationship>
    {
      Relationship.Create(
        Guid.NewGuid(), Guid.NewGuid(), RelationshipType.Friend, DateOnly.FromDateTime(date),
        DateOnly.FromDateTime(date), 50
      )
    };

    A.CallTo(() => _characterService.CreateRandomCharacterAsync(A<World>._, CancellationToken.None))
      .ReturnsNextFromSequence(randomCharacters.ToArray());

    A.CallTo(() => _relationshipService.GenerateRelationships(randomCharacters, DateOnly.FromDateTime(date)))
      .Returns(generatedRelationships);

    // Act
    var world = await _worldService.CreateNewWorldAsync(date, playerCharacter, gameSettings, CancellationToken.None);

    // Assert
    Assert.NotNull(world);
    Assert.Contains(playerCharacter, world.Characters);
    foreach (var npc in randomCharacters)
    {
      Assert.Contains(npc, world.Characters);
    }

    Assert.Equal(gameSettings.GetNpcCount() + 1, world.Characters.Count);
    Assert.Equal(generatedRelationships.Count, world.Relationships.Count);
    Assert.All(generatedRelationships, r => Assert.Contains(r, world.Relationships));
  }

  [Fact]
  public async Task CreateNewWorld_ShouldThrow_WhenSpawnRoomIsNull()
  {
    // Arrange
    var date = new DateTime(2025, 4, 24);
    var playerCharacter = new CharacterBuilder().Build();
    var gameSettings = GameSettings.Create(NpcDensity.Low);

    A.CallTo(() => _roomService.GetPlayerSpawnAsync(CancellationToken.None)).Returns((Room?) null);

    // Act
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      _worldService.CreateNewWorldAsync(date, playerCharacter, gameSettings, CancellationToken.None)
    );

    // Assert
    Assert.Equal("No spawn room found.", ex.Message);
  }

  [Fact]
  public void AdvanceTime_ShouldAddMinutesToWorldCurrentDate()
  {
    // Arrange
    var initialDate = new DateTime(2025, 4, 24, 8, 0, 0);
    var playerCharacter = new CharacterBuilder().Build();
    var world = World.Create(initialDate, []);
    const int minutesToAdvance = 90;

    // Act
    _worldService.AdvanceTime(world, playerCharacter.Id, minutesToAdvance);

    // Assert
    Assert.Equal(initialDate.AddMinutes(minutesToAdvance), world.CurrentDate);
  }

  [Fact]
  public async Task CreateNewWorld_ShouldAddChildrenFromCouples()
  {
    // Arrange
    var date = new DateTime(2025, 4, 24);
    var nowD = DateOnly.FromDateTime(date);
    var playerCharacter = new CharacterBuilder().Build();

    var gameSettings = GameSettings.Create(NpcDensity.VeryLow);
    var location = Location.Create("Home");
    var room = Room.Create(location.Id, "Spawn", true);

    A.CallTo(() => _roomService.GetPlayerSpawnAsync(CancellationToken.None)).Returns(room);
    A.CallTo(() => _locationService.GetByIdAsync(location.Id, CancellationToken.None)).Returns(location);

    var npcs = Enumerable.Range(0, gameSettings.GetNpcCount()).Select(_ => new CharacterBuilder().Build()).ToList();

    A.CallTo(() => _characterService.CreateRandomCharacterAsync(A<World>._, CancellationToken.None))
      .ReturnsNextFromSequence(npcs.ToArray());

    // mark first two NPCs as romantic partners
    var parentA = npcs[0];
    var parentB = npcs[1];
    var romanticRel = Relationship.Create(parentA.Id, parentB.Id, RelationshipType.RomanticPartner, nowD, nowD, 80);

    // relationships generated by the first pass
    var baseRelationships = new List<Relationship> {romanticRel};

    A.CallTo(() => _relationshipService.GenerateRelationships(npcs, nowD)).Returns(baseRelationships);

    // stub child generation: one child + its parent/child relationships
    var kid = new CharacterBuilder().WithBirthDate(new DateOnly(2014, 1, 1)).Build();
    var kidR = new List<Relationship>
    {
      Relationship.Create(parentA.Id, kid.Id, RelationshipType.Parent, kid.BirthDate, nowD, 50),
      Relationship.Create(kid.Id, parentA.Id, RelationshipType.Child, kid.BirthDate, nowD, 50)
    };

    A.CallTo(() => _relationshipService.GenerateChildrenFromCouplesAsync(
        A<List<(Character, Character)>>._, A<World>._, CancellationToken.None
      )
    ).Returns((new List<Character> {kid}, kidR));

    // Act
    var world = await _worldService.CreateNewWorldAsync(date, playerCharacter, gameSettings, CancellationToken.None);

    // Assert
    // player + npcs + kid
    Assert.Equal(gameSettings.GetNpcCount() + 1 + 1, world.Characters.Count);
    Assert.Contains(kid, world.Characters);

    // base relationships + kid relationships
    Assert.Equal(baseRelationships.Count + kidR.Count, world.Relationships.Count);
    Assert.All(kidR, r => Assert.Contains(r, world.Relationships));

    // verify service calls
    A.CallTo(() => _relationshipService.GenerateChildrenFromCouplesAsync(
        A<List<(Character, Character)>>._, world, CancellationToken.None
      )
    ).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public async Task CreateNewWorld_ShouldSelectCouplesFromRomanticRelationships()
  {
    // Arrange
    var date = new DateTime(2025, 4, 24);
    var nowD = DateOnly.FromDateTime(date);
    var player = new CharacterBuilder().Build();
    var location = Location.Create("TestLoc");
    var room = Room.Create(location.Id, "Room", true);

    A.CallTo(() => _roomService.GetPlayerSpawnAsync(CancellationToken.None)).Returns(room);
    A.CallTo(() => _locationService.GetByIdAsync(location.Id, CancellationToken.None)).Returns(location);

    var gameSettings = GameSettings.Create(NpcDensity.VeryLow);
    var npcs = Enumerable.Range(0, gameSettings.GetNpcCount()).Select(_ => new CharacterBuilder().Build()).ToList();

    A.CallTo(() => _characterService.CreateRandomCharacterAsync(A<World>._, CancellationToken.None))
      .ReturnsNextFromSequence(npcs.ToArray());

    var a = npcs[0];
    var b = npcs[1];
    var romanticRel = Relationship.Create(
      a.Id < b.Id ? a.Id : b.Id, a.Id < b.Id ? b.Id : a.Id, RelationshipType.RomanticPartner, nowD, nowD, 100
    );

    A.CallTo(() => _relationshipService.GenerateRelationships(npcs, nowD)).Returns([romanticRel]);
    A.CallTo(() => _relationshipService.GenerateChildrenFromCouplesAsync(
        A<List<(Character, Character)>>._, A<World>._, CancellationToken.None
      )
    ).Returns(([], []));

    // Act
    var world = await _worldService.CreateNewWorldAsync(date, player, gameSettings, CancellationToken.None);

    // Assert
    Assert.Equal(gameSettings.GetNpcCount() + 1, world.Characters.Count);
  }

  [Fact]
  public void AdvanceTime_ShouldMoveNpc_WhenScheduleEntryExists()
  {
    // Arrange
    var initial = new DateTime(2025, 1, 6, 8, 0, 0);
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();

    var world = World.Create(initial, [player, npc]);

    var streetId = Guid.NewGuid();
    var roomId = (Guid?) null;

    var entry = new ScheduleEntry(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), streetId, roomId);
    var schedule = Schedule.Create(npc.Id, [entry]);

    world.SetSchedules([schedule]);

    // Act
    _worldService.AdvanceTime(world, player.Id, 10);

    // Assert
    Assert.Equal(initial.AddMinutes(10), world.CurrentDate);
    Assert.Equal(streetId, npc.LocationId);
    Assert.Equal(roomId, npc.RoomId);
  }

  [Fact]
  public void AdvanceTime_ShouldNotMoveNpc_WhenNoScheduleEntry()
  {
    // Arrange
    var initial = new DateTime(2025, 4, 24, 8, 0, 0);
    var npc = new CharacterBuilder().Build();
    var player = new CharacterBuilder().Build();

    var world = World.Create(initial, [player, npc]);

    world.SetSchedules([]);

    // Act
    _worldService.AdvanceTime(world, player.Id, 30);

    // Assert
    Assert.Equal(initial.AddMinutes(30), world.CurrentDate);
    Assert.Null(npc.LocationId);
    Assert.Null(npc.RoomId);
  }

  #endregion
}
