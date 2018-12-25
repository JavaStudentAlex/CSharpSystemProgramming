using System.IO;

namespace Lab4
{
    public class ResultsWriter
    {
        private StreamWriter fileWriter;

        public ResultsWriter(string pathToResultFile)
        {
            fileWriter = new StreamWriter(pathToResultFile);
        }

        public void WriteResultToFile(string word)
        {
            fileWriter.WriteLine(word);
        }

        public void Finish()
        {
            fileWriter.Close();
        }
    }
}