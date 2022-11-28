namespace Jha.Services.TweetsCollectorService.Services.Storage;

using System;

/// <summary>
/// The storage interface.
/// </summary>
/// <typeparam name="T">A storage model class.</typeparam>
public interface IRepository<T> : IEnumerable<T> where T : class
{
    /// <summary>
    /// Adds a new entity to the storage.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>An added entity.</returns>
    public T Add(T entity);

    /// <summary>
    /// Gets an entity from the storage which satisfies the predicate.
    /// </summary>
    /// <param name="predicate">Search query predicate.</param>
    /// <returns>The enityt if found, otherwise null.</returns>
    public T? GetFirstOrDefault(Func<T, bool> predicate);

    /// <summary>
    /// Get all entities from the storage which satisfies the predicate.
    /// </summary>
    /// <param name="predicate">Search query predicate. If null, all entities will be returned.</param>
    /// <returns>A colletion of entities.</returns>
    public IEnumerable<T> GetAllWhere(Func<T, bool>? predicate = null);

    /// <summary>
    /// Removes an entity from the storage which satisfies the predicate.
    /// </summary>
    /// <param name="predicate">Search query predicate.</param>
    /// <returns>The removed entity if found, otherwise null.</returns>
    public T? RemoveFirstOrDefault(Func<T, bool> predicate);

    /// <summary>
    /// Count entities in the store.
    /// </summary>
    /// <returns>The entities count.</returns>
    public int Count();

    /// <summary>
    /// Clears the storage.
    /// </summary>
    public void Clear();
}

