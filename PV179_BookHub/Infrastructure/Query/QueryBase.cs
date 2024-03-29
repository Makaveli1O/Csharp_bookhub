﻿using Infrastructure.Exceptions;
using Infrastructure.Query.Filters;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Query;

public class QueryBase<TEntity, TKey> : IQuery<TEntity, TKey> where TEntity : class
{
    private IQueryable<TEntity> _query;

    public IUnitOfWork UnitOfWork {  get; set; }
    public IFilter<TEntity>? Filter { get; set; }
    public QueryParams? QueryParams { get; set; }


    public QueryBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
        _query = unitOfWork
            .GetRepositoryByEntity<TEntity, TKey>()
            .AsQueryable();
    }

    public async Task<QueryResult<TEntity>> ExecuteAsync()
    {
        var result = await _query.ToListAsync();

        var queryResult = new QueryResult<TEntity>()
        {
            Items = result,
            PageSize = QueryParams?.PageSize ?? PagingParameters.defaultPageSize,
            PagingEnabled = true,
            PageNumber = QueryParams?.PageNumber ?? PagingParameters.defaultPageNumber
        };

        return queryResult;
    }

    public async Task<int> CountTotalAsync()
    {
        return await _query.CountAsync();
    }

    public IQuery<TEntity, TKey> Page(int pageToFetch, int pageSize)
    {
        _query = _query.Skip((pageToFetch - 1) * pageSize).Take(pageSize);

        return this;
    }

    //https://stackoverflow.com/questions/1689199/c-sharp-code-to-order-by-a-property-using-the-property-name-as-a-string/67630860#67630860
    public IQuery<TEntity, TKey> SortBy(string sortAccordingTo, bool ascending)
    {
        if (sortAccordingTo == string.Empty)
        {
            return this;
        }
        
        var param = Expression.Parameter(typeof(TEntity));
        var memberAccess = Expression.Property(param, sortAccordingTo);
        if (memberAccess == null)
        {
            throw new NoSuchPropertyException(sortAccordingTo);
        }
        var convertedMemberAccess = Expression.Convert(memberAccess, typeof(object));
        var orderPredicate = Expression.Lambda<Func<TEntity, object>>(convertedMemberAccess, param);

        if (ascending)
        {
            _query = _query.OrderBy(orderPredicate);
        }
        else
        {
            _query = _query.OrderByDescending(orderPredicate);
        }

        return this;
    }

    public IQuery<TEntity, TKey> Where(Expression<Func<TEntity, bool>>? filter = null)
    {
        if (filter != null)
        {
            _query = _query.Where(filter);
        }

        return this;
    }

    public IQuery<TEntity, TKey> Include(params Expression<Func<TEntity, object?>>[] includes)
    {
        if (includes != null)
        {
            _query = includes
                .Aggregate(_query, (current, include) => current.Include(include));
        }

        return this;
    }
}
