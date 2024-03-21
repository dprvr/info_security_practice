using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

using System.Threading.Tasks;

namespace FC
{
    [Command()]
    internal class DecryptFileCommand : ICommand
    {
        
        public ValueTask ExecuteAsync(IConsole console)
        {

            return default;
        }
    }
}