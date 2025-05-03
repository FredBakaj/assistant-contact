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
                                                       From the large text, highlight information that matches my contact's interests: "{0}". Create a short text divided into paragraphs, where each paragraph is a concise description of one relevant aspect from the text. The text should be compact, focusing on key ideas that I can easily mention in a conversation with a contact. Exclude anything that doesn't relate to the interests indicated

                                                       {1}
                                                       """;
}
