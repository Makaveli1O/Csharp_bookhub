﻿using AutoMapper;
using BusinessLayer.DTOs.BookStore.Create;
using BusinessLayer.DTOs.BookStore.View;
using BusinessLayer.Services;
using BusinessLayer.Services.Book;
using BusinessLayer.Services.InventoryItem;

namespace BusinessLayer.Facades.BookStore;

public class InventoryItemFacade : BaseFacade, IInventoryItemFacade
{
    private readonly IInventoryItemService _inventoryItemService;
    private readonly IBookService _bookService;
    private readonly IGenericService<BookStoreEntity, long> _bookStoreService;

    public InventoryItemFacade(
        IMapper mapper,
        IInventoryItemService inventoryItemService,
        IGenericService<BookStoreEntity, long> bookStoreService,
        IBookService bookService)
        : base(mapper)
    {
        _inventoryItemService = inventoryItemService;
        _bookService = bookService;
        _bookStoreService = bookStoreService;
    }

    public async Task<DetailedInventoryItemViewDto> CreateInventoryItem(CreateInventoryItemDto createInventoryItemDto)
    {
        return _mapper.Map<DetailedInventoryItemViewDto>(
            await _inventoryItemService.CreateAsync(
                _mapper.Map<InventoryItemEntity>(createInventoryItemDto)));
    }

    public async Task DeleteInventoryItem(long id)
    {
        await _inventoryItemService.DeleteAsync(await _inventoryItemService.FindByIdAsync(id));
    }

    public async Task<IEnumerable<DetailedInventoryItemViewDto>> GetAllInventoryItems()
    {
        return _mapper.Map<List<DetailedInventoryItemViewDto>>(await _inventoryItemService.FetchAllAsync());
    }

    public async Task<DetailedInventoryItemViewDto> GetInventoryItem(long id)
    {
        return _mapper.Map<DetailedInventoryItemViewDto>(await _inventoryItemService.FindByIdAsync(id));
    }

    public async Task<DetailedInventoryItemViewDto> UpdateInventoryItem(long id, CreateInventoryItemDto updateInventoryItemDto)
    {
        var inventoryItem = await _inventoryItemService.FindByIdAsync(id);

        // will throw not found exception if entities do not exist
        await _bookService.FindByIdAsync(updateInventoryItemDto.BookId);
        await _bookStoreService.FindByIdAsync(updateInventoryItemDto.BookStoreId);

        inventoryItem.InStock = updateInventoryItemDto.InStock;
        inventoryItem.LastRestock = updateInventoryItemDto.LastRestock;
        inventoryItem.BookStoreId = updateInventoryItemDto.BookStoreId;
        inventoryItem.BookId = updateInventoryItemDto.BookId;
        await _inventoryItemService.UpdateAsync(inventoryItem);
       
        return _mapper.Map<DetailedInventoryItemViewDto>(inventoryItem);
    }
}