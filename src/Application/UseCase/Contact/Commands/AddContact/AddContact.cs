using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Application.Manager.Ai;
using AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;
using AssistantContract.Domain.Entities;
using Microsoft.VisualBasic;

namespace AssistantContract.Application.UseCase.Contact.Commands.AddContact;

public record AddContactCommand : IRequest
{
    public long UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string Description { get; set; } = null!;
}

public class AddContactCommandValidator : AbstractValidator<AddContactCommand>
{
    public AddContactCommandValidator()
    {
    }
}

public class AddContactCommandHandler : IRequestHandler<AddContactCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAiManager _aiManager;

    public AddContactCommandHandler(IApplicationDbContext context, IAiManager aiManager)
    {
        _context = context;
        _aiManager = aiManager;
    }

    public async Task Handle(AddContactCommand request, CancellationToken cancellationToken)
    {
        var countContact =
            await _context.Contact.CountAsync(x => x.UserId == request.UserId, cancellationToken: cancellationToken);
        if (countContact <= 25)
        {
            var keywords =
                await _aiManager.QueryToLlmModelAsync(string.Format(GetRecommendationPromptField.PromptDetectKeywords,
                    request.Description));
            
            var maxNumberContact = countContact > 0
                ? await _context.Contact.Where(x => x.UserId == request.UserId)
                    .MaxAsync(x => x.ContactNumber, cancellationToken: cancellationToken)
                : 0;

            await _context.Contact.AddAsync(
                new ContactEntity()
                {
                    UserId = request.UserId,
                    ContactNumber = maxNumberContact + 1,
                    Name = request.Name,
                    Phone = request.Phone,
                    Description = request.Description,
                    KeywordDescription = keywords
                }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new InvalidOperationException("User The user has more than 25 contacts");
        }
    }
}
