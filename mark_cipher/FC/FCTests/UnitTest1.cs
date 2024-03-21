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

        [TestCase("кот", "рыжий")]
        [TestCase("котанр", "операнд")]
        [TestCase("кот¬а", "аргумент")]
        [TestCase("кот¬", "анализ")]
        [TestCase("парк", "марксизм")]
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