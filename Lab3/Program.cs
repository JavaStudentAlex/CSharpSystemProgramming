﻿using System;
using System.IO;
using System.Text;

namespace Lab3
{
    internal class Program
    {
        enum SymbolType
        {
            Digit,
            Letter,
            RightBracket,
            LeftBracket,
            Semicolomn,
            UnknownSymbol,
            FileEnd,
            Space,
            StartSymbol
        }

        enum LexemsType
        {
            Begin = 257,
            End,
            If,
            Do,
            While,
            SimpleWord,
            SyntaxLexem
        }

        public static void Main(string[] args)
        {
            string pathToResourceFile = "..\\..\\resources\\resource.db";
            string pathToResultFile = "..\\..\\result.db";

            InstructionsTreatment treater = new InstructionsTreatment(pathToResourceFile);
            ResultsWriter writer = new ResultsWriter(pathToResultFile);

            while (true)
            {
                treater.SetNextLexemTypeToTreater();
                LexemsType type = treater.CurrentLexem;
                string word = treater.CurrentWord;
                writer.WriteResultToFile(type, word);
                if (word.Trim() == "@") break;
            }

            treater.Finish();
            writer.Finish();
        }

        class InstructionsTreatment
        {
            private StreamReader fileReader;
            private char currentChar;
            private LexemsType currentLexemType;
            private string currentWord;
            private SymbolType currentSymbolType=SymbolType.StartSymbol;

            public string CurrentWord
            {
                get { return currentWord; }
            }

            public LexemsType CurrentLexem
            {
                get { return currentLexemType; }
            }


            public InstructionsTreatment(string pathToResourceFile)
            {
                fileReader = new StreamReader(pathToResourceFile);
            }

            public void SetNextLexemTypeToTreater()
            {
                SetNextWordToTreater();
                switch (currentWord)
                {
                    case "begin" : currentLexemType = LexemsType.Begin; return;
                    case "end" : currentLexemType = LexemsType.End; return; 
                    case "if" : currentLexemType = LexemsType.If; return;
                    case "do" : currentLexemType = LexemsType.Do; return;
                    case "while" : currentLexemType = LexemsType.While; return;
                    default:
                        if (currentWord.Length > 1)
                        {
                            currentLexemType = LexemsType.SimpleWord;
                            return;
                        }
                        else
                        {
                            currentLexemType = LexemsType.SyntaxLexem;
                            return;
                        }
                }
            }

            private void SetNextWordToTreater()
            {
                StringBuilder builder = new StringBuilder();

                if (AreEqulSymbolTypes(SymbolType.StartSymbol))
                {
                    LoadAndAnalizeSymbol();
                }
                
                while (true)
                {
                    builder.Append(currentChar);
                    SymbolType prevType = currentSymbolType;
                    LoadAndAnalizeSymbol();
                    if (!IsWordConrinue() || !AreEqulSymbolTypes(prevType)) break;
                }

                Console.WriteLine("word : " + builder.ToString());
                currentWord = builder.ToString();
            }

            private void LoadAndAnalizeSymbol()
            {
                SetNextSymbolToTreater();
                SetCurrentCharTypeToTreater();
            }

            private bool AreEqulSymbolTypes(SymbolType type)
            {
                return type == currentSymbolType;
            }

            private bool IsWordConrinue()
            {
                if (currentSymbolType==SymbolType.Space || currentSymbolType==SymbolType.Semicolomn ||
                    currentSymbolType==SymbolType.LeftBracket || currentSymbolType==SymbolType.RightBracket || 
                    currentSymbolType==SymbolType.FileEnd)
                {
                    return false;
                }
                return true;
            }

            private void SetCurrentCharTypeToTreater()
            {
                switch (currentChar)
                {
                    case ';': currentSymbolType = SymbolType.Semicolomn; 
                        return;
                    case '(': currentSymbolType = SymbolType.LeftBracket; 
                        return;
                    case ')': currentSymbolType = SymbolType.RightBracket;
                        return;
                    case '@': currentSymbolType = SymbolType.FileEnd;
                        return;
                    case ' ' : currentSymbolType = SymbolType.Space;
                        return;
                    case '\t' : currentSymbolType = SymbolType.Space;
                        return;
                    case '\n' : currentSymbolType = SymbolType.Space;
                        return;
                    default:
                        if (Char.IsDigit(currentChar))
                        {
                            currentSymbolType = SymbolType.Digit;
                            return;
                        }
                        else if (Char.IsLetter(currentChar))
                        {
                            currentSymbolType = SymbolType.Letter;
                            return;
                        }
                        else
                        {
                            currentSymbolType = SymbolType.UnknownSymbol;
                            return;
                        }
                }
            }
            

            private void SetNextSymbolToTreater()
            {
                currentChar = (char)fileReader.Read();
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

            public void WriteResultToFile(LexemsType type, string word)
            {
                fileWriter.WriteLine(type.ToString() + " - "+word);
            }

            public void Finish()
            {
                fileWriter.Close();
            }
        }
    }
}