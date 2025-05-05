namespace AssistantContract.Application.UseCase.Contact.Queries.GetRecommendation;

public abstract class GetRecommendationPromptField
{
    public const string PromptDetectKeywords = """
                                               Generate 20 common search queries based on the description of interests
                                               
                                               Example:
                                               Interests:
                                               Likes to watch historical videos on YouTube, currently watching the Fyb channel a lot. Also likes to watch CS 2 videos, watches Evelon stream recordings. Reads self-development books, specifically “The 48 Laws of Strength” and “Don't Eat Alone”.
                                               
                                               As a result, you need to output a similar pattern:
                                               
                                               Best historical documentaries on YouTube; Summary of 48 Laws of Power by Robert Greene; Key lessons from Don't Eat Alone by Keith Ferrazzi; Top YouTube history channels; How to improve social skills networking books; CS2 strategies and tips for ranked matches; 
                                               
                                               ---
                                               Inerests:
                                               {0}
                                               """;

}
