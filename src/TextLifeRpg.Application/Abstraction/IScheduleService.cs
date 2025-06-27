using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

public interface IScheduleService
{
  Task<List<Schedule>> GenerateSchedulesAsync(
    IEnumerable<Character> characters, DateOnly currentDate, CancellationToken cancellationToken
  );
}
