﻿using AutoMapper;
using BusinessLayer.DTOs.Author.Create;
using BusinessLayer.DTOs.Author.Filter;
using BusinessLayer.DTOs.Author.View;
using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.Services.Author;
using Infrastructure.Query;
using Infrastructure.Query.Filters.EntityFilters;

namespace BusinessLayer.Facades.Author;

public class AuthorFacade : BaseFacade, IAuthorFacade
{
    private readonly IAuthorService _authorService;
    public AuthorFacade(IMapper mapper, IAuthorService authorService) : base(mapper, null, null)
    {
        _authorService = authorService;
    }

    public async Task<DetailedAuthorViewDto> CreateAuthorAsync(CreateAuthorDto createAuthorDto)
    {
        var author = _mapper.Map<AuthorEntity>(createAuthorDto);

        await _authorService.CreateAsync(author);

        return _mapper.Map<DetailedAuthorViewDto>(author);
    }

    public async Task DeleteAuthorAsync(long id)
    {
        var author = await _authorService.FindByIdAsync(id);

        await _authorService.DeleteAsync(author);
    }

    public async Task<FilterResultDto<GeneralAuthorViewDto>> FetchFilteredAuthorsAsync(AuthorFilterDto authorFilterDto)
    {
        var queryResult = await _authorService
            .FetchFilteredAsync(_mapper.Map<AuthorFilter>(authorFilterDto),
                                _mapper.Map<QueryParams>(authorFilterDto));

        return _mapper.Map<FilterResultDto<GeneralAuthorViewDto>>(queryResult);
    }

    public async Task<DetailedAuthorViewDto> FindAuthorByIdAsync(long id)
    {
        var author = await _authorService.FindByIdAsync(id);

        return _mapper.Map<DetailedAuthorViewDto>(author);
    }

    public async Task<IEnumerable<GeneralAuthorViewDto>> GetAllAuthorsAsync()
    {
        return _mapper.Map<List<GeneralAuthorViewDto>>(await _authorService.FetchAllAsync());
    }

    public async Task<DetailedAuthorViewDto> UpdateAuthorAsync(long id, CreateAuthorDto updateAuthorDto)
    {
        var author = await _authorService.FindByIdAsync(id);

        author.Name = updateAuthorDto.Name ?? author.Name;
        author.Biography = updateAuthorDto.Biography ?? author.Biography;

        await _authorService.UpdateAsync(author);
        return _mapper.Map<DetailedAuthorViewDto>(author);
    }
}
