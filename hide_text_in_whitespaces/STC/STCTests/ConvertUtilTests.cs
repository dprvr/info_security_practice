using NUnit.Framework;

using STC;

namespace STCTests
{
    [TestFixture]
    public class ConvertUtilTests
    {
        [Test]
        [TestCase(new byte[] { 12, 122, 21 })]
        [TestCase(new byte[] { 123, 122, 122 })]
        [TestCase(new byte[] { 19, 12, 18 })]
        public void BytesToBitsConvertingTest(byte[] bytes)
        {
            var bits = bytes.ToBits();
            var actualBytes = bits.ToBytes();
            Assert.AreEqual(bytes, actualBytes);
        }

    }
}
