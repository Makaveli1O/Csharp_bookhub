using BookHubWebAPI.Middleware;
using BookHubWebAPI.Swagger;
using BusinessLayer.Facades.Address;
using BusinessLayer.Facades.Author;
using BusinessLayer.Facades.Book;
using BusinessLayer.Facades.Publisher;
using BusinessLayer.Facades.WishList;
using BusinessLayer.Facades.User;
using BusinessLayer.Mappers;
using BusinessLayer.Services;
using BusinessLayer.Services.Author;
using BusinessLayer.Services.Book;
using BusinessLayer.Services.Publisher;
using DataAccessLayer.Data;
using DataAccessLayer.Models.Logistics;
using DataAccessLayer.Models.Preferences;
using DataAccessLayer.Models.Publication;
using DataAccessLayer.Models.Account;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<BookHubDbContext>(options =>
{
    var folder = Environment.SpecialFolder.LocalApplicationData;
    var dbPath = Path.Join(Environment.GetFolderPath(folder), "BookHub.db");

    options
        .UseSqlite(
            $"Data Source={dbPath}",
            x => x.MigrationsAssembly("DAL.SQLite.Migrations")
            )
        .UseLazyLoadingProxies();
});

builder.Services.AddScoped<IUnitOfWork, BookHubUnitOfWork>();

builder.Services.AddScoped<IGenericService<Address, long>, GenericService<Address, long>>();
builder.Services.AddScoped<IAddressFacade, AddressFacade>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorFacade, AuthorFacade>();

builder.Services.AddScoped<IGenericService<Publisher, long>, PublisherService>();
builder.Services.AddScoped<IPublisherFacade, PublisherFacade>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookFacade, BookFacade>();

builder.Services.AddScoped<IGenericService<WishList, long>, GenericService<WishList, long>>();
builder.Services.AddScoped<IGenericService<WishListItem, long>, GenericService<WishListItem, long>>();
builder.Services.AddScoped<IWishListFacade, WishListFacade>();

builder.Services.AddScoped<IGenericService<User, long>, GenericService<User, long>>();
builder.Services.AddScoped<IUserFacade, UserFacade>();

builder.Services.AddAutoMapper(typeof(AddressProfile));
builder.Services.AddAutoMapper(typeof(BookProfile));
builder.Services.AddAutoMapper(typeof(BookReviewProfile));
builder.Services.AddAutoMapper(typeof(BookStoreProfile));
builder.Services.AddAutoMapper(typeof(OrderProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(WishListItemProfile));
builder.Services.AddAutoMapper(typeof(WishListProfile));
builder.Services.AddAutoMapper(typeof(AuthorBookAssociationProfile));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter the API key as follows: topsecret",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    c.OperationFilter<FormatQueryParameterOperationFilter>(
         "format",
         "The format of the response",
         "json",
         new List<string> {"json", "xml"},
         false);
});

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseMiddleware<TokenAuthenticationMiddleware>();

app.UseMiddleware<XmlResponseConverterMiddleware>();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
