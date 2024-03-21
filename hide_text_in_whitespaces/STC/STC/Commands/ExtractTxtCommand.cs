using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;

using STC.Services;
using STC.Validators;

using System;
using System.Threading.Tasks;

namespace STC.Commands
{
    [Command("extract", Description = "Extracts txt-file from specified file-container.")]
    public class ExtractTxtCommand : ICommand
    {
        [CommandParameter(0, Name = "file-container path", Description = "txt file-container which text contains hidden txt-file", Validators = new Type[] { typeof(CorrectFilePath), typeof(TxtFile), typeof(FileExist) })]
        public string ContainerFilePath { get; init; }

        [CommandParameter(1, Name = "path to save extracted file", Description = "file path that will be used for extracted txt-file saving", Validators = new Type[] { typeof(CorrectFilePath), typeof(TxtFile) })]
        public string ExtractedTxtSaveFilePath { get; init; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            var hidingService = new TxtFilesHidingService(new WhitespacesSecretSpeech());
            try
            {
                hidingService.ExtractTxt(ContainerFilePath, ExtractedTxtSaveFilePath);
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
            console.Output.WriteLine("\tSuccesfully extracted...");
            return default;
        }
    }
}
