using NUnit.Framework;
using FC.Algo;

namespace FCTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCase("���", "�����")]
        [TestCase("������", "�������")]
        [TestCase("�����", "��������")]
        [TestCase("����", "������")]
        [TestCase("����", "��������")]
        [Test]
        public void Test1(string key, string message)
        {
            var mark = new MarkCryptor(key);
            var code = mark.Encrypt(message);
            var actual = mark.Decrypt(code);

            Assert.AreEqual(message, actual);
        }
    }
}