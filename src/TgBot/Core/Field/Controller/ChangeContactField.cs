namespace AssistantContract.TgBot.Core.Field.Controller;

public abstract class ChangeContactField
{
    public const string ChangeContactState = nameof(ChangeContactState);
    public const string ChangeContactAction = nameof(ChangeContactAction);
    public const string ChangeContactInputDescriptionAction = nameof(ChangeContactInputDescriptionAction);
    public const string ChangeContactSkipInputDescriptionKeyboard = "Skip description";
    public const string ChangeContactInputPersonalInfoAction = nameof(ChangeContactInputPersonalInfoAction);
    public const string ChangeContactSkipInputPersonalInfoKeyboard = "Skip personal info";
    public const string ChangeContactInputTimeSpanAction = nameof(ChangeContactInputTimeSpanAction);
    public const string ChangeContactSaveChangeContactAction = nameof(ChangeContactSaveChangeContactAction);
    public const string InputTimeSpanCallback = nameof(InputTimeSpanCallback);
    public const string SkipInputTimeSpanCallback = nameof(SkipInputTimeSpanCallback);
    public const string ChangeContactSavedContactKeyboard = "Save changes";
    public const string ChangeContactCanceledSaveContactKeyboard = "Cancel changes";
}
