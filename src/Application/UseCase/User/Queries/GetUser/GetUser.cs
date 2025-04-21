using AssistantContract.Application.Common.Interfaces;

namespace AssistantContract.Application.UseCase.User.Queries.GetUser;

public record GetUserQuery : IRequest<UserDto>
{
    public long UserId { get; set; }
}

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
    }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Id == request.UserId)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new InvalidOperationException($"user with id {request.UserId} was not found");
        }

        return user;
    }
}
