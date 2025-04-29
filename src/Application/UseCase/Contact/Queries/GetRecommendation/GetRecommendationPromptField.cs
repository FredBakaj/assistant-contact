namespace AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;

public abstract class GetRecommendationPromptField
{
    public const string PromptDetectKeywords = """
                                               Need to write keywords to generate news on the description of the person

                                               Example:
                                               A man with a lively mind and a broad outlook. Likes to spend time reading books, from classics to modern research. Interested in finance: follows markets, studies investment strategies and economic trends. Interested in world politics, seeks to understand how global events affect the economy and society. Calm, analytical, always looking to deepen his knowledge and see the interconnections in the world around him.
                                               result:
                                               Books; Finance; Investments

                                               Description:
                                               {0}
                                               """;

    public const string PromptGenerateRecommendation = """
                                                       Need to extract information from today's news that will be of interest to a person based on their description of the news.
                                                       Person description:
                                                       {0}
                                                       
                                                       Today`s news:
                                                       {1}
                                                       """;
}
