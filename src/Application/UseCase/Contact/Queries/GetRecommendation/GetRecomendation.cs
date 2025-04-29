using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Application.Manager.Ai;
using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;

public record RecommendationQuery : IRequest<ContactRecomendationModel>
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

public class RecommendationQueryHandler : IRequestHandler<RecommendationQuery, ContactRecomendationModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IAiManager _aiManager;

    public RecommendationQueryHandler(IApplicationDbContext context, IAiManager aiManager)
    {
        _context = context;
        _aiManager = aiManager;
    }

    public async Task<ContactRecomendationModel> Handle(RecommendationQuery request,
        CancellationToken cancellationToken)
    {
        var contact = await _context.Contact.FirstOrDefaultAsync(x =>
            x.UserId == request.UserId && x.ContactNumber == request.ContactNumber);
        if (contact != null)
        {
            
            
            var keywordsList = contact.KeywordDescription.Split(";");
            var random = new Random();
            var keyword = keywordsList[random.Next(keywordsList.Length)];
            

            var newsTexts = await _aiManager.QueryToNewsApiAsync(keyword);

            var promptNewsText = "\n\nToday's news post " + string.Join("\n\nToday's news post \n", newsTexts);

            var recommendationText =
                await _aiManager.QueryToLlmModelAsync(
                    string.Format(GetRecommendationPromptField.PromptGenerateRecommendation, contact!.Description,
                        promptNewsText));

            await _context.RecommendationsUser.AddAsync(new RecommendationsUserEntity() { ContactId = contact.Id },
                cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ContactRecomendationModel() { RecommendationText = recommendationText };
        }
        else
        {
            throw new InvalidOperationException("This contact does not exist for this user");
        }
    }
}
