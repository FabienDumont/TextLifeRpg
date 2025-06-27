using TextLifeRpg.Infrastructure;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class RepositoryBaseTests
{
  #region Methods

  [Fact]
  public void Ctor_ShouldThrowArgumentNullException_WhenContextIsNull()
  {
    // Act & Assert
    var ex = Assert.Throws<ArgumentNullException>(() => _ = new TestRepository(null!));
    Assert.Equal("context", ex.ParamName);
  }

  [Fact]
  public async Task SaveAsync_ShouldCallContextSaveChangesAsync()
  {
    // Arrange
    var context = A.Fake<ApplicationContext>();
    var cancellationToken = CancellationToken.None;
    var repo = new TestRepository(context);

    // Act
    await repo.SaveAsync(cancellationToken);

    // Assert
    A.CallTo(() => context.SaveChangesAsync(cancellationToken)).MustHaveHappenedOnceExactly();
  }

  #endregion

  private class TestRepository(ApplicationContext context) : RepositoryBase(context);
}
