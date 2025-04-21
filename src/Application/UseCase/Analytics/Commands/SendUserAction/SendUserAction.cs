using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.Analytics.Commands.SendUserAction;

public record SendUserActionCommand : IRequest
{
    public long UserId { get; set; }
    public string TypeAction { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string? Data { get; set; }
}

public class SendUserActionCommandValidator : AbstractValidator<SendUserActionCommand>
{
    public SendUserActionCommandValidator()
    {
    }
}

public class SendUserActionCommandHandler : IRequestHandler<SendUserActionCommand>
{
    private readonly IApplicationDbContext _context;

    public SendUserActionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SendUserActionCommand request, CancellationToken cancellationToken)
    {
        // var userAction = new AnalyticsUserActionEntity()
        // {
        //     UserId = request.UserId, TypeAction = request.TypeAction, Action = request.Action, Data = request.Data,
        // };
        // _context.AnalyticsUserAction.Add(userAction);
        // await _context.SaveChangesAsync(cancellationToken);
        await Task.FromResult("");
    }
}
