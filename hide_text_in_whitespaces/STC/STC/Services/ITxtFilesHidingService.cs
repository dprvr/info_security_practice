namespace STC.Services
{
    public interface ITxtFilesHidingService
    {
        void HideTxt(string containerPath, string hidingTxtPath, string txtContainerPath);
        void ExtractTxt(string txtContainerPath, string txtPath);
    }
}
