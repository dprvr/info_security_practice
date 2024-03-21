using System.IO;
using System.Linq;

namespace STC.Services
{
    public class TxtFilesHidingService : ITxtFilesHidingService
    {
        private readonly IHidingAlgorithm _hidingAlgorithm;

        public TxtFilesHidingService(IHidingAlgorithm hidingAlgorithm)
        {
            _hidingAlgorithm = hidingAlgorithm;
        }

        public void HideTxt(string containerPath, string hidingTxtPath, string txtContainerPath)
        {
            var containerText = File.ReadAllText(containerPath);
            var hidingMessage = File.ReadAllBytes(hidingTxtPath).ToBits().ToArray();
            var modifiedContainer = _hidingAlgorithm.HideMessage(containerText, hidingMessage);
            File.WriteAllText(txtContainerPath, modifiedContainer);
        }

        public void ExtractTxt(string txtContainerPath, string txtPath)
        {
            var containerText = File.ReadAllText(txtContainerPath);
            var txtBytes = _hidingAlgorithm.ExtractMessage(containerText).ToBytes();
            File.WriteAllBytes(txtPath, txtBytes);
        }
    }
}
