using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.UseCase.StateTree.Queries.GetStateAndAction;

public class StateAndActionDto
{
    public string State { get; set; } = null!;
    public string Action { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<StateTreeEntity, StateAndActionDto>();
        }
    }
}
