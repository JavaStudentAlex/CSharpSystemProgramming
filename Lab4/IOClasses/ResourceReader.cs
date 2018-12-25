using System.IO;

namespace Lab4
{
    public class ResourceReader
    {
        private StreamReader fileReader;

        public ResourceReader(string pathToResourceFile)
        {
            fileReader = new StreamReader(pathToResourceFile);
        }

        public char GetNextChar()
        {
            return (char) fileReader.Read();
        }

        public void Finish()
        {
            fileReader.Close();
        }
    }
}