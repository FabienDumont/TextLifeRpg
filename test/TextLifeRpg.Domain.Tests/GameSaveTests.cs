using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Domain.Tests;

public class GameSaveTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeCorrectly()
  {
    var playerCharacter = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [playerCharacter]);

    var save = GameSave.Create(playerCharacter, world);

    Assert.NotNull(save);
    Assert.NotEqual(Guid.Empty, save.Id);
    Assert.Equal(playerCharacter.Id, save.PlayerCharacterId);
    Assert.Equal(world, save.World);
    Assert.Equal(playerCharacter, save.PlayerCharacter);
    Assert.True(DateTime.UtcNow - save.SavedAt < TimeSpan.FromSeconds(1));
  }

  [Fact]
  public void Load_ShouldInitializeCorrectly()
  {
    var playerCharacter = new CharacterBuilder().Build();
    var id = Guid.NewGuid();
    var world = World.Create(DateTime.Now, [playerCharacter]);

    var save = GameSave.Load(id, playerCharacter.Id, world, []);

    Assert.Equal(id, save.Id);
    Assert.Equal(playerCharacter.Id, save.PlayerCharacterId);
    Assert.Equal(world, save.World);
    Assert.Equal(playerCharacter, save.PlayerCharacter);
  }

  [Fact]
  public void Load_ShouldThrow_WhenPlayerCharacterNotFound()
  {
    var id = Guid.NewGuid();
    var wrongPlayerId = Guid.NewGuid();
    var world = World.Create(DateTime.Now, []);

    var ex = Assert.Throws<InvalidOperationException>(() => GameSave.Load(id, wrongPlayerId, world, []));

    Assert.Equal("Player character not found in character list.", ex.Message);
  }

  [Fact]
  public void Create_ShouldThrow_WhenPlayerIsNull()
  {
    var world = World.Create(DateTime.Now, []);

    var ex = Assert.Throws<ArgumentNullException>(() => GameSave.Create(null!, world));

    Assert.Equal("playerCharacter", ex.ParamName);
  }

  [Fact]
  public void AddText_Should_Add_Text_With_Color_To_Save()
  {
    var playerCharacter = new CharacterBuilder().Build();
    var id = Guid.NewGuid();
    var world = World.Create(DateTime.Now, [playerCharacter]);
    var save = GameSave.Load(id, playerCharacter.Id, world, []);

    save.AddText(
      [
        new TextPart(CharacterColor.Blue, "Daniel: "),
        new TextPart(null, "Hello!")
      ]
    );

    Assert.Single(save.TextLines);
    var line = save.TextLines.First();
    Assert.Equal(2, line.TextParts.Count);
    Assert.Equal(CharacterColor.Blue, line.TextParts[0].Color);
    Assert.Equal("Daniel: ", line.TextParts[0].Text);
    Assert.Null(line.TextParts[1].Color);
    Assert.Equal("Hello!", line.TextParts[1].Text);
  }

  [Fact]
  public void ResetText_Should_Clear_All_TextLines()
  {
    var playerCharacter = new CharacterBuilder().Build();
    var id = Guid.NewGuid();
    var world = World.Create(DateTime.Now, [playerCharacter]);
    var save = GameSave.Load(id, playerCharacter.Id, world, []);

    save.AddText(
      [
        new TextPart(CharacterColor.Blue, "Daniel: "),
        new TextPart(null, "Hello!")
      ]
    );

    save.ResetText();

    Assert.Empty(save.TextLines);
  }

  [Fact]
  public void StartDialogue_ShouldSetInteractingNpcIdAndType()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);

    // Act
    save.StartDialogue(npc.Id);

    // Assert
    Assert.Equal(npc.Id, save.InteractingNpcId);
    Assert.Equal(npc, save.InteractingNpc);
    Assert.Equal(NpcInteractionType.Dialogue, save.NpcInteractionType);
  }

  [Fact]
  public void EndInteraction_ShouldClearInteractingNpcAndType()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id); // simulate active interaction

    // Act
    save.EndInteraction();

    // Assert
    Assert.Null(save.InteractingNpcId);
    Assert.Null(save.InteractingNpc);
    Assert.Null(save.NpcInteractionType);
  }

  #endregion
}
