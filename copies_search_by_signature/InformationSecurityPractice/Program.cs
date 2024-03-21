using System;
using System.IO;

namespace InformationSecurityPractice
{
    class Program
    {
        static void Main(string[] args)
        {            
            var dir = new DirectoryInfo($"{SignUtilities.GetPathToCurProject()}/test");
            var file = new FileInfo($"{SignUtilities.GetPathToCurProject()}/test/sm.txt");            
            var offset = (10, SeekOrigin.Begin);
            int signatureLen = 16;

            var match = file.ReadSignature(signatureLen, offset);
            var files = dir.GetAllFiles();
            var searched = files.SearchBySignature(match, offset);

            Console.WriteLine(searched.CastToString("\n"));            
            Console.ReadKey();
        }
    }
}
