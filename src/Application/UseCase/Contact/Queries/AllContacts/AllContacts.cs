using AssistantContract.Application.Common.Interfaces;

namespace AssistantContract.Application.UseCase.Contact.Queries.AllContacts;

public record AllContactsQuery : IRequest<AllContactsModel>
{
    public long UserId { get; set; }
}

public class AllContactsQueryValidator : AbstractValidator<AllContactsQuery>
{
    public AllContactsQueryValidator()
    {
    }
}

public class AllContactsQueryHandler : IRequestHandler<AllContactsQuery, AllContactsModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AllContactsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AllContactsModel> Handle(AllContactsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Contact.Where(x => x.UserId == request.UserId)
            .ProjectTo<ContactModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        return new AllContactsModel() { Contacts = result };
    }
}
