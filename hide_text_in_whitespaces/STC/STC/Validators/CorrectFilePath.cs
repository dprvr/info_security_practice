using CliFx.Extensibility;

namespace STC.Validators
{
    public class CorrectFilePath : BindingValidator<string>
    {
        public override BindingValidationError Validate(string value)
        {
            if (value.FilePathContainsInvalidCharacters())
                return Error($"The path '{value}' contains invalid characters.");
            if (value.ParentFileDirectoryExist())
                return Error($"The path '{value}' doesn't exist on pc, looks like some parent directories don't exist.");
            return Ok();
        }
    }
}