﻿using BusinessLayer.DTOs.Address.Create;
using BusinessLayer.DTOs.Address.View;
namespace BusinessLayer.Facades;

public interface IAddressFacade
{
    Task<DetailedAddressView?> FindAddressByIdAsync(long id);
    Task<bool> DeleteAddressByIdAsync(long id);
    Task<DetailedAddressView?> CreateAddressAsync(CreateAddressDto createAddressDto);
    Task<DetailedAddressView?> UpdateAddressAsync(long id, CreateAddressDto createAddressDto);
}
