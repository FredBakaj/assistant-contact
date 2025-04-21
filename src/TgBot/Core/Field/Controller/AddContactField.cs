namespace AssistantContract.TgBot.Core.Field.Controller;

public abstract class AddContactField
{
    public const string AddContactState = nameof(AddContactState);
    public const string AddContactAction = nameof(AddContactAction);
    public const string InputNameAction = nameof(InputNameAction);
    public const string InputPhoneAction = nameof(InputPhoneAction);
    public const string SkipInputPhoneKeyboard = "Skip";
    
    public const string InputDescriptionAction = nameof(InputDescriptionAction);
    public const string SaveContactAction = nameof(SaveContactAction);
    public const string SavedContactKeyboard = "Save contact";
    public const string CanceledSaveContactKeyboard = "Cancel";
}
