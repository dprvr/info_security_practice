using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InformationSecurityPractice.Stenography
{
    public static class Utilities
    {
        public delegate TContainer HideMessage<TMessage, TContainer>(TMessage message, TContainer container);
        public delegate TMessage ExtractHideMessage<TMessage, TContainer>(TContainer container);

        public static BitArray ToBitArray(this byte[] bytes)
        {
            return new BitArray(bytes);
        } 

        public (int Seek, byte[] text) ReadNecessaryTextSizeFromContainer(byte[] signature, int necessarySignatureCount )
        {
            byte[] text;
            int messageBitsCount = 10;
            long pos;
            using (StreamReader sr = new StreamReader(""))
            {
                text = Enumerable.Range(0, messageBitsCount)
                    .SelectMany(_ => Convert.FromBase64String(sr.ReadLine()))
                    .ToArray();
                pos = sr.BaseStream.Position;
            }
            return (pos, text.Length);
        }

        public byte[] HideMessageInText(byte[] bytes)
        {

        } 

        public void RewriteTextInContainer(byte[] textWithMessage, (int Seek, SeekOrigin origin) offset, int LengthOfReplacedText)
        {
            using(StreamReader sw = new StreamWriter(""))
            {
                sw.Rea
            }

            using (FileStream oldFs = new FileStream("old", FileMode.Open))
            using (FileStream newFs = new FileStream("", FileMode.Open))
            {
                oldFs.Re
                while (oldFs.Position != offset.Seek)
                {
                    newFs.W
                }
            }
        }

        public static void Main()
        {
            string message = "";
            var messageBits = Convert.FromBase64String(message).ToBitArray();
            int necessaryRowsCount = messageBits.Count;

            (int len, SeekOrigin from) = (1, SeekOrigin.Begin);
            

            FileInfo file = new FileInfo("");
            var sb = new StringBuilder();
            IEnumerable<string> text;
            

            using (StreamReader sr = new StreamReader(file.FullName))
            {
                sr.BaseStream.Seek(len, from);
                text = Enumerable.Range(0, necessaryRowsCount)
                    .Select(_ => sr.ReadLine())
                    .ToList();
            }

            text
                .Select(s => Convert.FromBase64String(s).ToBitArray().)
                .Select()

            using (FileStream fs = file.OpenRead())
            {
                fs.Seek(len, from);
                fs.Re
            }
            return block;

        }
    }
}
