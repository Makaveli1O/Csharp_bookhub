﻿using AutoMapper;
using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.DTOs.Publisher.Create;
using BusinessLayer.DTOs.Publisher.Filter;
using BusinessLayer.DTOs.Publisher.View;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using Infrastructure.Query.Filters.EntityFilters;
using Infrastructure.Query;

namespace BusinessLayer.Facades.Publisher
{
    public class PublisherFacade : BaseFacade, IPublisherFacade
    {
        private readonly IGenericService<PublisherEntity, long> _publisherService;

        public PublisherFacade(IMapper mapper, IGenericService<PublisherEntity, long> publisherService) : base(mapper, null, null)
        {
            _publisherService = publisherService;
        }

        public async Task<DetailedPublisherViewDto> CreatePublisherAsync(CreatePublisherDto createPublisherDto)
        {
            var publisher = _mapper.Map<PublisherEntity>(createPublisherDto);
            await _publisherService.CreateAsync(publisher);

            return _mapper.Map<DetailedPublisherViewDto>(publisher);
        }

        public async Task DeletePublisherAsync(long id)
        {
            var publisher = await _publisherService.FindByIdAsync(id);
            if (publisher.Books != null && publisher.Books.Any())
            {
                throw new RemoveErrorException(typeof(PublisherEntity), typeof(BookEntity));
            }

            await _publisherService.DeleteAsync(publisher);
        }

        public async Task<FilterResultDto<GeneralPublisherViewDto>> FetchFilteredPublishersAsync(PublisherFilterDto publisherFilterDto)
        {
            var queryResult = await _publisherService
                .FetchFilteredAsync(_mapper.Map<PublisherFilter>(publisherFilterDto),
                        _mapper.Map<QueryParams>(publisherFilterDto));

            return _mapper.Map<FilterResultDto<GeneralPublisherViewDto>>(queryResult);
        }

        public async Task<DetailedPublisherViewDto> FindPublisherByIdAsync(long id)
        {
            var publisher = await _publisherService.FindByIdAsync(id);

            return _mapper.Map<DetailedPublisherViewDto>(publisher);
        }

        public async Task<IEnumerable<GeneralPublisherViewDto>> GetAllPublishersAsync()
        {
            return _mapper.Map<List<GeneralPublisherViewDto>>(await _publisherService.FetchAllAsync());
        }

        public async Task<DetailedPublisherViewDto> UpdatePublisherAsync(long id, CreatePublisherDto updatePublisherDto)
        {
            var publisher = await _publisherService.FindByIdAsync(id);

            publisher.Name = updatePublisherDto.Name ?? publisher.Name;
            publisher.Country = updatePublisherDto.Country ?? publisher.Country;
            publisher.City = updatePublisherDto.City ?? publisher.City;
            publisher.YearFounded = updatePublisherDto.YearFounded ?? publisher.YearFounded;

            await _publisherService.UpdateAsync(publisher);
            return _mapper.Map<DetailedPublisherViewDto>(publisher);
        }
    }
}
