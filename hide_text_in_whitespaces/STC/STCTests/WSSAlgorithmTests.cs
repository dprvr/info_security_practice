using NUnit.Framework;

using STC.Services;

using System.IO;

namespace STCTests
{
    [TestFixture]
    public class WSSAlgorithmTests
    {
        private string Text { get; set; }

        [SetUp]
        public void SetUp()
        {
            Text = File.ReadAllText(@"C:\Users\rg\Desktop\cn.txt");
        }

        [TestCase(new bool[] { true, true, true, true })]
        [TestCase(new bool[] { false, false, false, false })]
        [TestCase(new bool[] { true, false, false, true, true, false, true })]
        [TestCase(new bool[] { true, false, true, false, false, false, true })]
        [Test]
        public void ShouldHideAndExtractSame(bool[] message)
        {
            var wss = new WhitespacesSecretSpeech();

            var filledContainer = wss.HideMessage(Text, message);
            var actualMessage = wss.ExtractMessage(filledContainer);

            Assert.AreEqual(message, actualMessage);
        }

    }
}
