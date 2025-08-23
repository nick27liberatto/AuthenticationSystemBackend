namespace Application.Handlers
{
    using Application.Dtos.Response;
    using Application.Queries;
    using AutoMapper;
    using Domain.Interfaces;
    using Domain.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
    using System.Threading.Tasks;

    public class SearchUsersHandler : IRequestHandler<SearchUsersQuery, ActionResult<IEnumerable<UserResponseDto>>>
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;

        public SearchUsersHandler(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<UserResponseDto>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllAsync();
            
            if(!string.IsNullOrEmpty(request.Search))
            {
                users = users.Where(x => x.Username.Contains(request.Search, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            return new OkObjectResult(_mapper.Map<IEnumerable<UserResponseDto>>(users)); 
        }
    }
}
