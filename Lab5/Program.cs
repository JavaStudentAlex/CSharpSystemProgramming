namespace Lab5
{
    class Program
    {
        StreamReader sr;
        StreamWriter sw;
        char nch = '\n';
        int lex;
        string[] TNM = new string[400];
        int ptn = 0;
        int lval;

        public Program(string nm1, string nm2)
        {
            sr = new StreamReader(nm1);
            sw = new StreamWriter(nm2);
        }
        char getc()
        {
            return (char)sr.Read();
        }
        void get()
        {
            while (!sr.EndOfStream)
            {
                while (Char.IsWhiteSpace(nch) || Char.IsControl(nch))
                { 
                    nch = getc();
                }
                
                if (Char.IsLetter(nch))
                {
                    word();
                }
                else if (Char.IsDigit(nch))
                {
                    number();
                }
                else if (nch == '(' || nch==')' || nch == ',' || nch== ';' || nch == '='|| nch == '+' ||nch=='-' || nch == '*' || nch=='/' || nch =='%' )
                {
                    lex = nch;
                    Console.WriteLine("{0} is a speacial symbol lex={1}", nch, lex);
                    sw.WriteLine("{0} is a speacial symbol lex={1}", nch, lex);
                    nch = getc();
                }
                
            }
        }
        void word()
        {
            string tx = " "+nch;
            while (!sr.EndOfStream)
            {
                nch = getc();
                if (Char.IsLetter(nch) || Char.IsDigit(nch))
                {
                    tx += nch;
                }
                else
                {
                    tx += ' ';
                    break;
                }
                
            }
            string[] serv = { "begin", "end", "if", "then", "while", "do", "return", "read", "print", "int", "const"  };
            int[] cdl = { (int)words.BEGINL, (int)words.ENDL, (int)words.IFL, (int)words.THENL,
                        (int)words.WHILEL, (int)words.DOL, (int)words.RETRL, (int)words.READL,
                        (int)words.PRITL, (int)words.INTL, (int)words.CONSTL };
            string txx = tx.Trim();
            for (int i=0; i<serv.Length; i++)
            {
                if (serv[i]==txx)
                {
                    lex = cdl[i];
                    Console.WriteLine("{0} is a key word lex={1}", txx, lex);
                    sw.WriteLine("{0} is a key word lex={1}", txx, lex);
                    return;
                }
            }
            lex = (int)words.IDEN;
            lval = add(txx);
            Console.WriteLine("{0} is an identifier lex={1} lval={2}", txx, lex, lval);
            sw.WriteLine("{0} is an identifier lex={1} lval={2}", txx, lex, lval);
        }
        int add(string nm)
        {
            for (int i = 0; i < TNM.Length; i++)
            {
                if (TNM[i] == nm)
                {
                    return i;
                }
            }
            try
            {
                TNM[ptn] = nm;
                return ptn++;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Переповнення таблиці TNM");
                return -1;
            }

        }
        void number()
        {
            lval = nch-'0';
            while (!sr.EndOfStream)
            {
                nch = getc();
                if (Char.IsDigit(nch))
                {
                    lval = 10 * lval + nch - '0';
                }
                else
                {
                    break;
                }

            }
            lex = (int)words.NUMB;
            Console.WriteLine("number lval={0} lex={1}", lval, lex);
            sw.WriteLine("number lval={0} lex={1}", lval, lex);
        }


        static void Main(string[] args)
        {
            string  name1="f1.txt";
            string  name2="f2.txt";
            Program ob = new Program(name1, name2);
            try
            {
                ob.get();
            }
            finally
            {
                if (ob.sr != null)
                    ob.sr.Dispose();
                if (ob.sw != null)
                    ob.sw.Dispose();
            }
            //Console.ReadKey();
        }
    }
}