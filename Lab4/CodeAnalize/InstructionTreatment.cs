using System;
using System.Text;

namespace Lab4
{
    public class InstructionsTreatment
    {
        private ResourceReader reader;
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


            public InstructionsTreatment(ResourceReader reader)
            {
                this.reader = reader;
            }

            public void SetNextLexemTypeToTreater()
            {
                SetNextWordToTreater();
                switch (currentWord)
                {
                    case "begin" : currentLexemType = LexemsType.Begin; break;
                    case "end" : currentLexemType = LexemsType.End; break; 
                    case "if" : currentLexemType = LexemsType.If; break;
                    case "do" : currentLexemType = LexemsType.Do; break;
                    case "while" : currentLexemType = LexemsType.While; break;
                    case "iden" : currentLexemType = LexemsType.Iden; break;
                    case "numb" : currentLexemType = LexemsType.Number; break;
                    default:
                        if (IsWord(currentWord))
                        {
                            currentLexemType = LexemsType.SimpleWord;
                            break;
                        }
                        else
                        {
                            currentLexemType = LexemsType.SyntaxLexem;
                            break;
                        }
                }
            }

            private Boolean IsWord(string lexemWord)
            {
                foreach (char tempChar in lexemWord.ToCharArray())
                {
                    if (!char.IsDigit(tempChar) && !char.IsLetter(tempChar))
                    {
                        return false;
                    }
                }

                return true;
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
                currentWord = builder.ToString();
                if (string.IsNullOrEmpty(currentWord.Trim()))
                {
                    SetNextWordToTreater();
                }
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
                    currentSymbolType==SymbolType.FileEnd || currentSymbolType==SymbolType.Comma)
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
                    case ',' : currentSymbolType = SymbolType.Comma;
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
                currentChar = reader.GetNextChar();
            }

            public void Finish()
            {
                reader.Finish();
            }
        }
}