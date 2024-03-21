using System.Collections;
using System.Linq;

namespace CRC
{
    internal class CRC8
    {
        private BitArray Divisor { get; }

        public CRC8()
        {
            Divisor = new BitArray(new byte[] { 0xd5 });
        }

        public byte ComputeCRC(byte[] message)
        {
            var augmentedMessage = AugmentMessage(message, 0);
            var crc = TryComputeCRC(augmentedMessage);
            var valid = IsCRCValid(crc, message);
            return crc;
        }

        private static byte[] AugmentMessage(byte[] message, byte augment)
        {
            var augmentedMessage = new byte[message.Length + 1];
            message.CopyTo(augmentedMessage, 0);
            augmentedMessage[^1] = augment;
            return augmentedMessage;
        }

        private bool IsCRCValid(byte messageCrc, byte[] message)
        {
            var crcAugmentedMessage = AugmentMessage(message, messageCrc);
            byte augmentedCrc = TryComputeCRC(crcAugmentedMessage);
            bool valid = augmentedCrc == 0;
            return valid;
        }

        private byte TryComputeCRC(byte[] message)
        {
            var rem = new BitArray(Divisor.Count, false);
            var messageBits = new BitArray(message.Reverse().ToArray());
            for (int i = messageBits.Count - 1; i >= 0; i--)
            {
                bool leftBitIsTrue = rem[rem.Count - 1];
                rem = rem.LeftShift(1);
                rem.Set(0, messageBits[i]);
                rem = leftBitIsTrue
                    ? rem.Xor(Divisor)
                    : rem;
            }
            byte[] remBytes = new byte[1];
            rem.CopyTo(remBytes, 0);
            return remBytes[0];
        }

    }
}
