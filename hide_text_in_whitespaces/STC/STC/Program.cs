using CliFx;

using System.Threading.Tasks;

namespace STC
{
    class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync();
        }
    }
}
