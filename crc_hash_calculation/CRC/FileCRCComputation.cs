using System;
using System.IO;
using System.Security;

namespace CRC
{
    internal class FileCRCComputation
    {
        private readonly string _filePath;
        private CRCComputingStatus _status;
        private string _checksum;
        
        public FileCRCComputation(string filePath)
        {
            FilePath = filePath;
            _checksum = string.Empty;
            _status = CRCComputingStatus.ComputedSuccess;
        }

        public string FilePath
        {
            get 
            {
                return _filePath;
            }
            private init
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"\"{nameof(FilePath)}\" не может быть неопределенным или пустым.", nameof(FilePath));
                _filePath = value;
            }
        }

        public CRCComputingResult ComputingResult
        {
            get
            {
                if (string.IsNullOrEmpty(_checksum))
                    ProcessComputing();
                return new CRCComputingResult
                {
                    ComputingStatus = _status,
                    CRCHashSum = _checksum,
                };
            }
        }
        
        private void ProcessComputing()
        {
            try
            {
                _checksum = ComputeFileCRC().ToString();
                _status = CRCComputingStatus.ComputedSuccess;
            }
            catch (IOException ioe) when (ioe is DirectoryNotFoundException || ioe is FileNotFoundException)  
            {
                _status = CRCComputingStatus.FileNotExistError;
            }
            catch (IOException)
            {
                _status = CRCComputingStatus.FileReadingError;
            }
            catch(SecurityException)
            {
                _status = CRCComputingStatus.FileAccessError;
            }
            catch
            {
                _status = CRCComputingStatus.UnexpectedError;
            }
        }

        private byte ComputeFileCRC()
        {
            byte[] fileContent; 
            using (FileStream fs = new(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, fileContent.Length);
            }
            var crc = new CRC8().ComputeCRC(fileContent);
            return crc;
        }

        public static implicit operator CRCComputingResult(FileCRCComputation computation)
        {
            return computation.ComputingResult;
        }
    }

    public readonly record struct CRCComputingResult
    {
        public CRCComputingStatus ComputingStatus { get; init; }
        public string CRCHashSum { get; init; }
    }

    public enum CRCComputingStatus
    {
        UnexpectedError,
        ComputedSuccess,
        FileAccessError,
        FileNotExistError,
        FileReadingError,
    }
}