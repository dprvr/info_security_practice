using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;

using STC.Services;
using STC.Validators;

using System;
using System.Threading.Tasks;

namespace STC
{
    [Command("hide", Description = "Hide text message in txt-file, using stenography algorithm.")]
    public class HideTxtCommand : ICommand
    {
        [CommandParameter(0, Name = "file-container", Description = "file-container which text will be used to hide message", Validators = new Type[] { typeof(FileExist), typeof(TxtFile) })]
        public string TextContainer { get; init; }

        [CommandParameter(1, Name = "hiding-message", Description = "txt-file which text will be hide in file-container text", Validators = new Type[] { typeof(CorrectFilePath), typeof(FileExist), typeof(TxtFile) })]
        public string HidingMessage { get; init; }

        [CommandParameter(2, Name = "file-output", Description = "txt-file that contains modified file-container text with hided message", Validators = new Type[] { typeof(CorrectFilePath), typeof(TxtFile) })]
        public string Output { get; init; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            var hidingService = new TxtFilesHidingService(new WhitespacesSecretSpeech());
            try
            {
                hidingService.HideTxt(TextContainer, HidingMessage, Output);
            }
            catch (ArgumentException exception)
            {
                throw new CommandException(exception.Message.BuildUserErrorMessage());
            }
            catch (UnauthorizedAccessException exception)
            {
                throw new CommandException(exception.Message.BuildUserErrorMessage());
            }
            catch
            {
                throw new CommandException("\t\tERROR:\nSome internal app error acquired, please check that input parameters correct and try again.");
            }
            console.Output.WriteLine("\tSuccesfully hide...");
            return default;
        }
    }
}
