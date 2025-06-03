using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.User.Commands.SendNotification;

public record SendNotificationCommand : IRequest
{
    public Func<long, int, string, string, Task> Action { get; set; } = null!;
}

public class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator()
    {
    }
}

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly IApplicationDbContext _context;

    public SendNotificationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            var contacts = await _context.Contact.Where(x => x.UserId == user.Id).ToListAsync(cancellationToken);
            var last24Hours = DateTime.UtcNow.AddHours(-24);

            var notifications = await _context.UserNotification
                .Where(x => x.UserId == user.Id && x.Created >= last24Hours)
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var contact in contacts)
            {
                if (notifications.All(x => x.ContactNumber != contact.ContactNumber))
                {
                    var days = (DateTime.UtcNow - contact.Created).Days;

                    if (contact.NotificationDayTimeSpan > 0 && days > 0 &&
                        days % contact.NotificationDayTimeSpan == 0)
                    {
                        await request.Action.Invoke(user.Id, contact.ContactNumber, contact.Name, contact.PersonalInfo ?? string.Empty);
                        await _context.UserNotification.AddAsync(
                            new UserNotificationEntity() { UserId = user.Id, ContactNumber = contact.ContactNumber },
                            cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
            }
        }
    }
}
