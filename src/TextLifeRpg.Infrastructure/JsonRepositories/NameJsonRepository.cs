using System.Text.Json;
using TextLifeRpg.Application.Abstraction.Repositories;

namespace TextLifeRpg.Infrastructure.JsonRepositories;

/// <summary>
/// Repository for retrieving character names from JSON files.
/// </summary>
public class NameJsonRepository(string dataDirectory) : INameRepository
{
  #region Implementation of INameRepository

  /// <inheritdoc />
  public async Task<IReadOnlyList<string>> GetFemaleNamesAsync(CancellationToken ct = default)
  {
    var path = Path.Combine(dataDirectory, "FemaleNames.json");
    var json = await File.ReadAllTextAsync(path, ct);

    return JsonSerializer.Deserialize<List<string>>(json) ?? [];
  }

  /// <inheritdoc />
  public async Task<IReadOnlyList<string>> GetMaleNamesAsync(CancellationToken ct = default)
  {
    var path = Path.Combine(dataDirectory, "MaleNames.json");
    var json = await File.ReadAllTextAsync(path, ct);

    return JsonSerializer.Deserialize<List<string>>(json) ?? [];
  }

  #endregion
}
