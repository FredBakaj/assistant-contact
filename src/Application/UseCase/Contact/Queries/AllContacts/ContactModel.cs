using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.Contact.Queries.AllContacts;

public class ContactModel
{
    public int ContactNumber { get; set; }
    public string Name { get; set; } = null!;
    public string? PersonalInfo { get; set; }
    public string Description { get; set; } = null!;
    public int NotificationDayTimeSpan { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ContactEntity, ContactModel>();
        }
    }

}
