using AssistantContract.Application.Common.Interfaces;

namespace AssistantContract.Application.UseCase.Contact.Commands.DeleteContact;

public record DeleteContactCommand : IRequest
{
    public long UserId { get; set; }
    public int ContactNumber { get; set; }
}

public class DeleteContactCommandValidator : AbstractValidator<DeleteContactCommand>
{
    public DeleteContactCommandValidator()
    {
    }
}

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact =
            await _context.Contact.FirstOrDefaultAsync(x =>
                x.UserId == request.UserId && x.ContactNumber == request.ContactNumber);

        _context.Contact.Remove(contact!);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
