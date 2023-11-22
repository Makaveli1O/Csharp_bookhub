﻿using AutoMapper;
using BusinessLayer.DTOs.Book.Create;
using BusinessLayer.DTOs.Book.View;
using DataAccessLayer.Models.Publication;
using Infrastructure.NaiveQuery;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace BookHubWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(CreateBookDto createBookDto)
    {
        var book = _mapper.Map<Book>(createBookDto);

        await _unitOfWork.BookRepository.AddAsync(book);
        await _unitOfWork.CommitAsync();

        return Created(
            new Uri($"{Request.Path}/{book.Id}", UriKind.Relative),
            _mapper.Map<DetailedBookViewDto>(book)
            );
    }

    [HttpPatch]
    [Route("{bookId}/{authorId}")]
    public async Task<IActionResult> AssignAuthorToBook(long bookId, long authorId)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
        var author = await _unitOfWork.AuthorRepository.GetByIdAsync(authorId);

        if (book == null || author == null) 
        {
            return NotFound();
        }

        var authorBookAssociation = new AuthorBookAssociation()
        {
            AuthorId = authorId,
            BookId = bookId
        };

        await _unitOfWork.AuthorBookAssociationRepository.AddAsync(authorBookAssociation);

        return Ok(
            _mapper.Map<DetailedBookViewDto>(book)
            );
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateBook(long id, CreateBookDto createBookDto)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
        if (book != null)
        {
            book.Title = createBookDto.Title ?? book.Title;
            book.ISBN = createBookDto.ISBN ?? book.ISBN;
            //book.Author = createBookDto.Author ?? book.Author;
            //book.Publisher = createBookDto.Publisher ?? book.Publisher;
            book.Description = createBookDto.Description ?? book.Description;
            book.BookGenre = createBookDto.BookGenre;
            book.Price = createBookDto.Price;

            _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.CommitAsync();
        }

        return Ok(
            _mapper.Map<DetailedBookViewDto>(book)
            );
    }

    [HttpGet]
    public async Task<IActionResult> FetchAll()
    {
        var books = await _unitOfWork.BookRepository.GetAllAsync();

        return Ok(
            _mapper.Map<List<GeneralBookViewDto>>(books)
            );
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FetchSingle(long id)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsync(id, x => x.Authors, x => x.Reviews, x => x.Publisher);

        return Ok(
            _mapper.Map<DetailedBookViewDto>(book)
            );
    }

    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FetchBooksByFilters([FromQuery] IDictionary<string, string> query)
    {
        var filter = new Infrastructure.NaiveQuery.Filters.BookFilter(query);
        IQuery<Book> query1 = new QueryBase<Book, long>(_unitOfWork)
        {
            CurrentPage = 1,
            Filter = filter,
            SortAccordingTo = "Price",
            UseAscendingOrder = false
        };

        query1.Include(x => x.Authors, x => x.Reviews);
        query1.Where(filter.CreateExpression());
        query1.Page(1, 20);
        query1.SortBy("Price", false);

        var res = await query1.ExecuteAsync();
        var books = res.Items;

        return Ok(
            _mapper.Map<IEnumerable<DetailedBookViewDto>>(books)
            );
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(long id)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
        if (book != null)
        {
            _unitOfWork.BookRepository.Delete(book);
            await _unitOfWork.CommitAsync();
        }
        return NoContent();
    }
}
