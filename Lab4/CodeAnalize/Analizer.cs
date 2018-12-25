using System;
using System.Collections.Generic;
using System.Text;
using Lab4.Exceptions;

namespace Lab4
{
    public class Analizer
    {
        private InstructionsTreatment treater;
        private HashSet<string> variables;
        private Dictionary<string, int> numberConstants;
        private Dictionary<LexemsType, List<string>> otherwords;
        private LexemsType currentLexemType;
        private string currentWord;

        public Analizer(string pathToResourceFile)
        {
            treater = new InstructionsTreatment(new ResourceReader(pathToResourceFile));
            InitRepos();
            Analize();
        }

        private void Analize()
        {
            while (true)
            {
                SetNextLexem();
                if (currentLexemType.Equals(LexemsType.Iden))
                {
                    FindIdents();
                    continue;
                }

                if (currentLexemType.Equals(LexemsType.Number))
                {
                    FindConstants();
                    continue;
                }
                if (currentWord.Trim() == "@") break;

                if (otherwords.ContainsKey(currentLexemType))
                {
                    List<string> list;
                    otherwords.TryGetValue(currentLexemType,out list);
                    list.Add(currentWord);
                }
                else
                {
                    List<string> list = new List<string>();
                    list.Add(currentWord);
                    otherwords.Add(currentLexemType,list);
                }
            }
            
            treater.Finish();
        }

        private void FindIdents()
        {
            while (true)
            {
                SetNextLexem();
                if(!currentLexemType.Equals(LexemsType.SimpleWord) || char.IsDigit(currentWord[0])) 
                {throw new ExpectedStatementException();}                   
                variables.Add(currentWord);
                
                
                SetNextLexem();
                if(currentWord.Equals(",")) continue;
                if(currentWord.Equals(";")) break;
                throw new ExpectedStatementException();
            }
        }

        private void FindConstants()
        {
            while (true)
            {
                SetNextLexem();
                if (!currentLexemType.Equals(currentLexemType == LexemsType.SimpleWord) ||
                    char.IsDigit(currentWord[0]))
                    throw new ExpectedStatementException();
                string varname = currentWord;
                SetNextLexem();
                if(!currentWord.Equals("=")) throw new ExpectedStatementException();

                int constantVal;
                SetNextLexem();
                if (!currentLexemType.Equals(LexemsType.SimpleWord) || Int32.TryParse(currentWord, out constantVal))
                    throw new ExpectedStatementException();
                numberConstants.Add(varname,constantVal);
                
                SetNextLexem();
                if(currentWord.Equals(",")) continue;
                if(currentWord.Equals(";")) break;
                throw new ExpectedStatementException();
            }
        }

        private void SetNextLexem()
        {
            treater.SetNextLexemTypeToTreater();
            currentLexemType = treater.CurrentLexem;
            currentWord = treater.CurrentWord;
        }

        private void InitRepos()
        {
            variables = new HashSet<string>();
            numberConstants = new Dictionary<string, int>();
            otherwords = new Dictionary<LexemsType, List<string>>();
        }

        public string GetIdentsFormattedString()
        {
            StringBuilder builder = new StringBuilder("Identificators : ");
            foreach (string var in variables)
            {
                builder.Append(var).Append(" ");
            }

            return builder.ToString();
        }

        public string GetConstantsFormattedString()
        {
            StringBuilder builder = new StringBuilder("Constants : ");
            foreach (KeyValuePair<string,int> pair in numberConstants)
            {
                builder.Append(pair.Key).Append("=").Append(pair.Value);
            }

            return builder.ToString();
        }

        public string GetOtherLexemsWordsFormattedString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<LexemsType,List<string>> pair in otherwords)
            {
                builder.Append(pair.Key.ToString()).Append(" : ");
                foreach (string word in pair.Value)
                {
                    builder.Append(word).Append(" ");
                }

                builder.Append('\n');
            }

            return builder.ToString();
        }
    }
}