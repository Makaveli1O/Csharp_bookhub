﻿using Infrastructure.Query;
using Infrastructure.Query.Filters;

namespace BusinessLayer.Services;

public interface IGenericService<TEntity, TKey> : IBaseService where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity, bool save = true);
    Task<TEntity> UpdateAsync(TEntity entity, bool save = true);
    Task<IEnumerable<TEntity>> FetchAllAsync();
    Task<QueryResult<TEntity>> FetchFilteredAsync(IFilter<TEntity> filter, QueryParams queryParams);
    Task<TEntity> FindByIdAsync(TKey id);
    Task DeleteAsync(TEntity entity, bool save = true);
}
