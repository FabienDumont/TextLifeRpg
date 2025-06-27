namespace TextLifeRpg.Application.InternalUtilities;

/// <summary>
/// Provides extension methods to simplify object-to-object mapping.
/// </summary>
public static class MapperExtensions
{
  #region Methods

  /// <summary>
  /// Maps a single input object to a result using the specified mapping function.
  /// </summary>
  /// <typeparam name="TInput">Type of the input object.</typeparam>
  /// <typeparam name="TResult">Type of the result object.</typeparam>
  /// <param name="input">The input object to map.</param>
  /// <param name="mapper">The mapping function.</param>
  /// <returns>The mapped result object.</returns>
  public static TResult Map<TInput, TResult>(this TInput input, Func<TInput, TResult> mapper)
  {
    return mapper(input);
  }

  /// <summary>
  /// Maps a collection of input objects to a list of results using the specified mapping function.
  /// </summary>
  /// <typeparam name="TInput">Type of the input objects.</typeparam>
  /// <typeparam name="TResult">Type of the result objects.</typeparam>
  /// <param name="input">The collection of input objects to map.</param>
  /// <param name="mapper">The mapping function.</param>
  /// <returns>A distinct list of mapped result objects.</returns>
  public static List<TResult> MapCollection<TInput, TResult>(
    this IEnumerable<TInput> input, Func<TInput, TResult> mapper
  )
  {
    return input.Select(mapper).Distinct().ToList();
  }

  #endregion
}
