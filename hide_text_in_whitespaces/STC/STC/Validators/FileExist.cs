using CliFx.Extensibility;

namespace STC.Validators
{
    public class FileExist : BindingValidator<string>
    {
        public override BindingValidationError Validate(string value)
        {
            if (!value.ExistingFilePath())
                return Error($"The file named '{value}' doesn't exist");
            return Ok();
        }
    }
}