using System;
using System.IO;
using System.Text;

namespace Lab2
{

    enum SymbolType
    {
        Digit,Letter,RightBracket, LeftBracket,Semicolomn, UnknownSymbol, FileEnd
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            string pathToResourceFile = "..\\..\\resources\\resource.db";
            string pathToResultFile = "..\\..\\result.db";
            
            CharsTreatment treater = new CharsTreatment(pathToResourceFile);
            ResultsWriter writer = new ResultsWriter(pathToResultFile);

            while (true)
            {
                SymbolType type = treater.GetNextCharType();
                writer.WriteResultToFile(type);
                if (type.Equals(SymbolType.FileEnd))
                {
                    break;
                }
            }
            treater.Finish();
            writer.Finish();
        }
    }

    class CharsTreatment
    {
        private StreamReader fileReader;
        

        public CharsTreatment(string pathToResourceFile)
        {
            fileReader = new StreamReader(pathToResourceFile);
        }

        public SymbolType GetNextCharType()
        {
            char symbol = GetNextSymbol();
            switch (symbol)
            {
                case ';' : return SymbolType.Semicolomn;
                case '(' : return SymbolType.LeftBracket;
                case ')' : return SymbolType.RightBracket;
                case '@' : return SymbolType.FileEnd;
                default :
                    if (Char.IsDigit(symbol)) return SymbolType.Digit;
                    else if (Char.IsLetter(symbol)) return SymbolType.Letter;
                    else return SymbolType.UnknownSymbol;
            }
        }

        public char GetNextSymbol()
        {
            return (char) fileReader.Read();
        }

        public void Finish()
        {
            fileReader.Close();
        }
    }

    class ResultsWriter
    {
        private StreamWriter fileWriter;

        public ResultsWriter(string pathToResultFile)
        {
            fileWriter = new StreamWriter(pathToResultFile);
        }

        public void WriteResultToFile(SymbolType type)
        {
            fileWriter.WriteLine(type.ToString());
        }

        public void Finish()
        {
            fileWriter.Close();
        }
    }
}