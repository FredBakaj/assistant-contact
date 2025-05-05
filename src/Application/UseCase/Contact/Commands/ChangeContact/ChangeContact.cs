using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Application.Manager.Ai;
using AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;

namespace AssistantContract.Application.UseCase.Contact.Commands.ChangeContact;

public record ChangeContactCommand : IRequest
{
    public long UserId { get; set; }
    public string? Description { get; set; }
    public int? NotificationDayTimeSpan { get; set; }
    public int ContactNumber { get; set; }
}

public class ChangeContactCommandValidator : AbstractValidator<ChangeContactCommand>
{
    public ChangeContactCommandValidator()
    {
    }
}

public class ChangeContactCommandHandler : IRequestHandler<ChangeContactCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAiManager _aiManager;

    public ChangeContactCommandHandler(IApplicationDbContext context, IAiManager aiManager)
    {
        _context = context;
        _aiManager = aiManager;
    }

    public async Task Handle(ChangeContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _context.Contact.FirstOrDefaultAsync(x =>
                x.UserId == request.UserId && x.ContactNumber == request.ContactNumber,
            cancellationToken: cancellationToken);
        
        if (!string.IsNullOrEmpty(request.Description))
        {
            var keywords =
                await _aiManager.QueryToLlmModelAsync(string.Format(GetRecommendationPromptField.PromptDetectKeywords,
                    request.Description));

            contact!.Description = request.Description;
            contact.KeywordDescription = keywords;
        }

        contact!.NotificationDayTimeSpan = request.NotificationDayTimeSpan ?? contact.NotificationDayTimeSpan;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
