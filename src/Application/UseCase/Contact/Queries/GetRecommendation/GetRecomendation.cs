using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Application.Manager.Ai;
using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;

public record RecommendationQuery : IRequest<ContactRecommendationModel>
{
    public long UserId { get; set; }
    public int ContactNumber { get; set; }
}

public class RecommendationQueryValidator : AbstractValidator<RecommendationQuery>
{
    public RecommendationQueryValidator()
    {
    }
}

public class RecommendationQueryHandler : IRequestHandler<RecommendationQuery, ContactRecommendationModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IAiManager _aiManager;

    public RecommendationQueryHandler(IApplicationDbContext context, IAiManager aiManager)
    {
        _context = context;
        _aiManager = aiManager;
    }

    public async Task<ContactRecommendationModel> Handle(RecommendationQuery request,
        CancellationToken cancellationToken)
    {
        var contact = await _context.Contact.FirstOrDefaultAsync(x =>
            x.UserId == request.UserId && x.ContactNumber == request.ContactNumber);
        if (contact != null)
        {
            
            
            var keywordsList = contact.KeywordDescription.Split(";");
            var random = new Random();
            var keyword = keywordsList[random.Next(keywordsList.Length)];
            

            var searchResponse = await _aiManager.QueryToGoogleSearchAsync(keyword);

            
            await _context.RecommendationsUser.AddAsync(new RecommendationsUserEntity() { ContactId = contact.Id },
                cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ContactRecommendationModel() { SearchResponse = searchResponse };
        }
        else
        {
            throw new InvalidOperationException("This contact does not exist for this user");
        }
    }
}
