interface IDatabase<T>
{
    Task<T> Read(object id);
    Task<List<T>> ReadAll();
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(object id);
}