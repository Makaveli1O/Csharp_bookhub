﻿using BusinessLayer.DTOs.Address.Create;
using BusinessLayer.DTOs.Address.View;

namespace BusinessLayer.Facades.Address;

public interface IAddressFacade
{
    Task<IEnumerable<DetailedAddressView>> GetAllAddressesAsync();

    Task<IEnumerable<GeneralAddressView>> GetAvailableAddressesForBookStoreAsync(long? bookStoreId);
    Task<DetailedAddressView> FindAddressByIdAsync(long id);
    Task DeleteAddressByIdAsync(long id);
    Task<DetailedAddressView> CreateAddressAsync(CreateAddressDto createAddressDto);
    Task<DetailedAddressView> UpdateAddressAsync(long id, CreateAddressDto createAddressDto);
}
