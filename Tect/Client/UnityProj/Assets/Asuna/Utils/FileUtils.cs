
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
        
    }
}