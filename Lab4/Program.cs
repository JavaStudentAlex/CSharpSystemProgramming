
namespace Lab4
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            string pathToResourceFile = "..\\..\\resources\\resource.db";
            string pathToResultFile = "..\\..\\result.db";
            
            Analizer analizer = new Analizer(pathToResourceFile);
            ResultsWriter writer = new ResultsWriter(pathToResultFile);

            writer.WriteResultToFile(analizer.GetIdentsFormattedString());
            writer.WriteResultToFile(analizer.GetConstantsFormattedString());
            writer.WriteResultToFile(analizer.GetOtherLexemsWordsFormattedString());
            writer.Finish();
        }
    }
}