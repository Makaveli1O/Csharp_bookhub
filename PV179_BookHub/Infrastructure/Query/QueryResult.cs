﻿namespace Infrastructure.Query;

public class QueryResult<TEntity>
{
    public long TotalItemsCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public IList<TEntity> Items { get; set; } = new List<TEntity>();
    public bool PagingEnabled { get; set; }
}
