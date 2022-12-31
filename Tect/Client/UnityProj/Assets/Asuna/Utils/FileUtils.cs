
using System.IO;
using System.Text;

namespace Asuna.Utils
{
    public static class FileUtils
    {
        public static void WriteContentToFileSync(string filePath, string content, bool overwrite)
        {
            var data = Encoding.ASCII.GetBytes(content);
            WriteContentToFileSync(filePath, data, overwrite);
        }
        
        public static void WriteContentToFileSync(string filePath, byte[] content, bool overwrite)
        {
            if (File.Exists(filePath))
            {
                if (!overwrite)
                {
                    throw new IOException("file path is exist");
                }
                File.Delete(filePath);
            }

            using var fp = File.Open(filePath, FileMode.CreateNew);
            fp.Write(content);
            fp.Close();
        }

        public static byte[] ReadBytesFromFileSync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new IOException("file path is not exist");
            }

            return File.ReadAllBytes(filePath);
        }

        public static string ReadContentFromFileSync(string filePath)
        {
            byte[] data = ReadBytesFromFileSync(filePath);
            return Encoding.ASCII.GetString(data);
        }

        public static void MakeSureDirectory(string path, bool clean = true)
        {
            bool exist = Directory.Exists(path);
            if (exist && clean)
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }
    }
}