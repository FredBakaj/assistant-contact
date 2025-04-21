using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.User.Queries.GetUser;

public class UserDto
{
    public string Name { get; set; } = null!;


    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserEntity, UserDto>();
        }
    }
}
