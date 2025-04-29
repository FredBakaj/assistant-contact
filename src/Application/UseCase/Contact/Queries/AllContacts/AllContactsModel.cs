using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.Contact.Queries.AllContacts;

public class AllContactsModel
{
    public List<ContactModel> Contacts { get; set; } = null!;
}
