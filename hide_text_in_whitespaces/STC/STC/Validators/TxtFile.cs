using CliFx.Extensibility;

namespace STC.Validators
{
    public class TxtFile : BindingValidator<string>
    {
        public override BindingValidationError Validate(string value)
        {
            if (value.TxtFilePath())
                return Error($"The file named '{value}' must be txt-file.");
            return Ok();
        }
    }
}
