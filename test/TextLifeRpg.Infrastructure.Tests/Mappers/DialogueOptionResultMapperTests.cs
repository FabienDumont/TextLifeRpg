using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class DialogueOptionResultMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapCorrectly()
  {
    // Arrange
    var id = Guid.NewGuid();
    var dialogueOptionId = Guid.NewGuid();
    const bool endDialogue = true;

    var dataModel = new DialogueOptionResultDataModel
    {
      Id = id,
      DialogueOptionId = dialogueOptionId,
      EndDialogue = endDialogue
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(dialogueOptionId, domain.DialogueOptionId);
    Assert.Equal(endDialogue, domain.EndDialogue);
  }

  [Fact]
  public void ToDataModel_ShouldMapCorrectly()
  {
    // Arrange
    var id = Guid.NewGuid();
    var dialogueOptionId = Guid.NewGuid();
    const bool endDialogue = false;

    var domain = DialogueOptionResult.Load(id, dialogueOptionId, endDialogue);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(dialogueOptionId, dataModel.DialogueOptionId);
    Assert.Equal(endDialogue, dataModel.EndDialogue);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapAllCorrectly()
  {
    // Arrange
    var dataModels = new[]
    {
      new DialogueOptionResultDataModel
      {
        Id = Guid.NewGuid(),
        DialogueOptionId = Guid.NewGuid(),
        EndDialogue = false
      },
      new DialogueOptionResultDataModel
      {
        Id = Guid.NewGuid(),
        DialogueOptionId = Guid.NewGuid(),
        EndDialogue = true
      }
    };

    // Act
    var domains = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(dataModels.Length, domains.Count);
    for (int i = 0; i < dataModels.Length; i++)
    {
      Assert.Equal(dataModels[i].Id, domains[i].Id);
      Assert.Equal(dataModels[i].DialogueOptionId, domains[i].DialogueOptionId);
      Assert.Equal(dataModels[i].EndDialogue, domains[i].EndDialogue);
    }
  }

  #endregion
}
