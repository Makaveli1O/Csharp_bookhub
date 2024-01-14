﻿using AutoMapper;
using BusinessLayer.DTOs.Book.View;
using BusinessLayer.DTOs.WishList.Create;
using BusinessLayer.DTOs.WishList.View;
using BusinessLayer.Services;
using BusinessLayer.Services.Book;
using BusinessLayer.Services.WishList;
using DataAccessLayer.Models.Publication;
using Infrastructure.Query;
using Infrastructure.Query.Filters.EntityFilters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer.Facades.WishList;

public class WishListFacade : BaseFacade, IWishListFacade
{
    private readonly IWishListService _wishListService;
    private readonly IGenericService<WishListItemEntity, long> _wishListItemService;
    private readonly IGenericService<BookEntity, long> _bookService;

    public WishListFacade(IMapper mapper,
                          IWishListService wishListService,
                          IGenericService<WishListItemEntity, long> wishListItemService,
                          IGenericService<BookEntity, long> bookService,
                          IMemoryCache memoryCache)
        : base(mapper, memoryCache, "wishlist-")
    {
        _wishListService = wishListService;
        _wishListItemService = wishListItemService;
        _bookService = bookService;
    }

    public async Task<GeneralWishListViewDto> CreateWishListAsync(CreateWishListDto createWishListDto)
    {
        var wishList = _mapper.Map<WishListEntity>(createWishListDto);

        var createdWishList = await _wishListService.CreateAsync(wishList);

        return _mapper.Map<GeneralWishListViewDto>(createdWishList);
    }

    public async Task<GeneralWishListItemViewDto> CreateWishListItemAsync(CreateWishListItemDto createWishListItemDto)
    {
        var wishListItem = _mapper.Map<WishListItemEntity>(createWishListItemDto);
        var book = _mapper.Map<GeneralBookViewDto>(await _bookService.FindByIdAsync(createWishListItemDto.BookId));
        
        var createdWishListItem = await _wishListItemService.CreateAsync(wishListItem);
        var wishListItemView = _mapper.Map<GeneralWishListItemViewDto>(createdWishListItem);
        wishListItemView.Book = book;

        return wishListItemView;
    }
    public async Task<GeneralWishListViewDto> UpdateWishListAsync(long id, string? wishListDescription)
    {
        var wishList = await _wishListService.FindByIdAsync(id);

        wishList.Description = wishListDescription;
        
        _memoryCache?.Set(GetMemoryCacheKey(id), wishList);
        await _wishListService.UpdateAsync(wishList);

        return _mapper.Map<GeneralWishListViewDto>(wishList);
    }
    
    public async Task<GeneralWishListItemViewDto> UpdateWishListItemAsync(long id, uint preferencePriority)
    {
        var wishListItem = await _wishListItemService.FindByIdAsync(id);
        var book = _mapper.Map<GeneralBookViewDto>(await _bookService.FindByIdAsync(wishListItem.BookId));

        wishListItem.PreferencePriority = preferencePriority;
        var updatedWishListItemDto = _mapper.Map<GeneralWishListItemViewDto>(await _wishListItemService.UpdateAsync(wishListItem));
        updatedWishListItemDto.Book = book;

        return updatedWishListItemDto;
    }
    
    public async Task DeleteWishListAsync(long id)
    {
        var wishList = await _wishListService.FindByIdAsync(id);
        _memoryCache?.Remove(wishList);
        await _wishListService.DeleteAsync(wishList);
    }
   
    public async Task DeleteWishListItemAsync(long id)
    {
        var wishListItem = await _wishListItemService.FindByIdAsync(id);
        await _wishListItemService.DeleteAsync(wishListItem);
    }
    
    public async Task DeleteWishListItemsAsync(long wishListId)
    {
        var wishList = await _wishListService.FindByIdAsync(wishListId);

        if (wishList.WishListItems != null) 
        {
            var deleteTasks = wishList.WishListItems.Select(item => _wishListItemService.DeleteAsync(item));
            await Task.WhenAll(deleteTasks);
        }
    }

    public async Task<IEnumerable<GeneralWishListItemViewDto>> FetchAllItemsFromWishListAsync(long wishListId)
    {
        var wishList = await _wishListService.FindByIdAsync(wishListId);
        return _mapper.Map<List<GeneralWishListItemViewDto>>(wishList?.WishListItems);
    }
  
    public async Task<GeneralWishListItemViewDto> FetchSingleItemFromWishListAsync(long itemId)
    {
        return _mapper.Map<GeneralWishListItemViewDto>(await _wishListItemService.FindByIdAsync(itemId));
    }
    
    public async Task<GeneralWishListViewDto> FetchWishListAsync(long id)
    {
        if (!(_memoryCache?.TryGetValue(GetMemoryCacheKey(id), out var cachedWishList) ?? false))
        {
            cachedWishList = await _wishListService.FindByIdAsync(id);
            _memoryCache?.Set(GetMemoryCacheKey(id), cachedWishList);
        }

        return _mapper.Map<GeneralWishListViewDto>(cachedWishList);
    }

    public async Task<IEnumerable<GeneralWishListViewDto>> FetchAllByUserIdAsync(long userId)
    {
        return _mapper.Map<IEnumerable<GeneralWishListViewDto>>(await _wishListService.FetchAllByUserIdAsync(userId));
    }
}
