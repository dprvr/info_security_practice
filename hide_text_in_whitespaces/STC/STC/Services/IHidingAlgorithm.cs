using System.Collections.Generic;

namespace STC.Services
{
    public interface IHidingAlgorithm
    {
        string HideMessage(string container, IReadOnlyList<bool> message);
        IReadOnlyList<bool> ExtractMessage(string container);
    }
}
