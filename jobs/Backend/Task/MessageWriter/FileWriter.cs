using System;
using System.IO;


namespace ExchangeRateUpdater.MessageWriter
{
    public class FileWriter : IWriter
    {
        private readonly string _folderPath;
        private string _fileName;

        public FileWriter(string folderPath, string file)
        {
            _folderPath = folderPath;
            _fileName = file == string.Empty ? $"{DateTime.Now.ToString("MMddyyyy")}" : $"{file}_{DateTime.Now.ToString("MMddyyyy")}";
        }

        public void WriteMessage(string message)
        {
            string filePath = _folderPath == string.Empty ? $"{_fileName}.txt" : $"{_folderPath}/{_fileName}.txt";

            DirectoryInfo di = new DirectoryInfo(_folderPath);
            if (!di.Exists) di.Create();

            FileInfo fi = new(filePath);
            if (!fi.Exists)
            {
                using StreamWriter sw = fi.CreateText();
                sw.WriteLine(message);
            }
            else
            {
                using StreamWriter sw = fi.AppendText();
                sw.WriteLine(message);
            }

            
        }
    }
}
