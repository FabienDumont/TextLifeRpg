using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class MovementNarrationMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    var movementId = Guid.NewGuid();
    var text = "You walk into the street.";

    var dataModel = new MovementNarrationDataModel
    {
      Id = id,
      MovementId = movementId,
      Text = text
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(movementId, domain.MovementId);
    Assert.Equal(text, domain.Text);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<MovementNarrationDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        MovementId = Guid.NewGuid(),
        Text = "You step outside."
      },
      new()
      {
        Id = Guid.NewGuid(),
        MovementId = Guid.NewGuid(),
        Text = "You return to your home."
      }
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);
    for (var i = 0; i < domainModels.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domainModels[i].Id);
      Assert.Equal(dataModels[i].MovementId, domainModels[i].MovementId);
      Assert.Equal(dataModels[i].Text, domainModels[i].Text);
    }
  }

  [Fact]
  public void ToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    var movementId = Guid.NewGuid();
    var text = "You sneak into the alley.";

    var domain = MovementNarration.Load(id, movementId, text);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(movementId, dataModel.MovementId);
    Assert.Equal(text, dataModel.Text);
  }

  #endregion
}
