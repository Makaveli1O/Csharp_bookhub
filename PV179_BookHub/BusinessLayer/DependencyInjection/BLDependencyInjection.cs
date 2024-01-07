﻿using BusinessLayer.Facades.Address;
using BusinessLayer.Facades.Author;
using BusinessLayer.Facades.Book;
using BusinessLayer.Facades.BookReview;
using BusinessLayer.Facades.BookStore;
using BusinessLayer.Facades.Order;
using BusinessLayer.Facades.Publisher;
using BusinessLayer.Facades.User;
using BusinessLayer.Facades.WishList;
using BusinessLayer.Mappers;
using BusinessLayer.Services.Author;
using BusinessLayer.Services.Book;
using BusinessLayer.Services.BookReview;
using BusinessLayer.Services.InventoryItem;
using BusinessLayer.Services.Order;
using BusinessLayer.Services.Publisher;
using BusinessLayer.Services;
using DataAccessLayer.Models.Account;
using DataAccessLayer.Models.Logistics;
using DataAccessLayer.Models.Preferences;
using DataAccessLayer.Models.Publication;
using DataAccessLayer.Models.Purchasing;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.DependencyInjection;

public static class BLDependencyInjection
{
    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AddressProfile));
        services.AddAutoMapper(typeof(BookProfile));
        services.AddAutoMapper(typeof(BookReviewProfile));
        services.AddAutoMapper(typeof(BookStoreProfile));
        services.AddAutoMapper(typeof(OrderProfile));
        services.AddAutoMapper(typeof(UserProfile));
        services.AddAutoMapper(typeof(WishListItemProfile));
        services.AddAutoMapper(typeof(WishListProfile));
        services.AddAutoMapper(typeof(AuthorBookAssociationProfile));
    }

    private static void RegisterServicesAndFacades(IServiceCollection services)
    {
        services.AddScoped<IGenericService<Address, long>, GenericService<Address, long>>();
        services.AddScoped<IAddressFacade, AddressFacade>();

        services.AddScoped<IInventoryItemService, InventoryItemService>();
        services.AddScoped<IInventoryItemFacade, InventoryItemFacade>();

        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IAuthorFacade, AuthorFacade>();

        services.AddScoped<IGenericService<Publisher, long>, PublisherService>();
        services.AddScoped<IPublisherFacade, PublisherFacade>();

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookFacade, BookFacade>();

        services.AddScoped<IGenericService<WishList, long>, GenericService<WishList, long>>();
        services.AddScoped<IGenericService<WishListItem, long>, GenericService<WishListItem, long>>();
        services.AddScoped<IWishListFacade, WishListFacade>();

        services.AddScoped<IGenericService<User, long>, GenericService<User, long>>();
        services.AddScoped<IUserFacade, UserFacade>();

        services.AddScoped<IGenericService<OrderItem, long>, GenericService<OrderItem, long>>();

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderFacade, OrderFacade>();
        services.AddScoped<IBookReviewService, BookReviewService>();
        services.AddScoped<IBookReviewFacade, BookReviewFacade>();

        services.AddScoped<IGenericService<BookStore, long>, GenericService<BookStore, long>>();
        services.AddScoped<IBookStoreFacade, BookStoreFacade>();
    }

    public static void RegisterBLDependencies(this IServiceCollection services)
    {
        RegisterMappers(services);
        RegisterServicesAndFacades(services);
    }
}