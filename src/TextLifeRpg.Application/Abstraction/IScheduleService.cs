using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Represents a service for generating schedules based on a set of characters and a specific date.
/// </summary>
public interface IScheduleService
{
  /// <summary>
  /// Asynchronously generates a list of schedules for a collection of characters based on the provided date.
  /// </summary>
  /// <param name="characters">The collection of characters for whom schedules will be generated.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests during the operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a list of generated schedules for the characters.</returns>
  Task<List<Schedule>> GenerateSchedulesAsync(IEnumerable<Character> characters, CancellationToken cancellationToken);
}
