namespace Lab7
{
    enum words {BEGINL = 257, ENDL, IFL, THENL, WHILEL, DOL, RETRL, READL, PRITL, INTL, CONSTL, IDEN, NUMB};
    struct odc {
        public string name;
        public int what;
        public int val;
    }
    class Program
    {
        StreamReader sr;
        StreamWriter sw;
        char nch = '\n';
        int lex;
        string[] TNM = new string[400];
        int ptn = 0;
        int lval;
        int nst = 0;

        int[] st = new int[500];
        int cgv = 0;
        int clv = 0;
        odc[] TOB = new odc[30];
        int pto = 0;
        int ptol = 0;
        int ut = 1;

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
                    if (nch == '\n')
                    {
                        nst++;
                    }
                    nch = getc();
                }
                
                if (Char.IsLetter(nch))
                {
                    word();
                    return;
                }
                else if (Char.IsDigit(nch))
                {
                    number();
                    return;
                }
                else if (nch == '(' || nch==')' || nch == ',' || nch== ';' || nch == '='|| nch == '+' ||nch=='-' || nch == '*' || nch=='/' || nch =='%' )
                {
                    lex = nch;
                    Console.WriteLine("{0} is a speacial symbol lex={1}", nch, lex);
                    sw.WriteLine("{0} is a speacial symbol lex={1}", nch, lex);
                    nch = getc();
                    return;
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


        public void exam(int lx)
        {
            if (lex != lx)
            {
                Console.WriteLine("Не співпадають лексеми lex = {0} та lx = {1} в рядку nst ={2}", lex, lx, nst);
                sw.WriteLine("Не співпадають лексеми lex = {0} та lx = {1} в рядку nst ={2}", lex, lx, nst);
            }
            get();
        }

        void newob(String nm, int wt, int vl)
        {
            int pe, p;
            pe = (ut==1) ? 0: ptol;
            for (p = pto - 1; p >= pe; p--)
            {
                if (String.Compare(nm, TOB[p].name) == 0)
                {
                    Console.WriteLine("Описано двічі");
                    return;
                }
            }
            if (pto >= 100) {
                Console.WriteLine("Переповнення TOB");
                return;
            }
            TOB[pto].name = nm;
            TOB[pto].what = wt;
            TOB[pto].val = vl;
            pto++; 
            return;
        }

        int findob(String nm) {
            int p;
            for (p = pto - 1; p >= 0; p--) {
                if (String.Compare(nm, TOB[p].name) == 0) {
                    return p;
                }
            }
            Console.WriteLine("Не описано " + nm);
            return p;
        }



        void prog()
        {
            //Console.WriteLine("prog");
            while (!sr.EndOfStream)
            {
                switch (lex)
                {
                    case (int)words.IDEN:
                        dfunc();
                        break;
                    case (int)words.INTL:
                        dvarb();
                        break;
                    case (int)words.CONSTL:
                        dconst();
                        break;
                    default:
                        Console.WriteLine("Помилка в nst ="+ nst +" лексема lex ="+ lex);
                        sw.WriteLine("Помилка в nst =" + nst + " лексема lex =" + lex);
                        break;
                }
            }
        }

        void dconst()
        {
            //Console.WriteLine("dconst");
            do
            {
                get();
                cons();
            } while (lex == ',');
            exam(';');
        }

        void cons()
        {
            //Console.WriteLine("cons");
            String nm = TNM[lval];
            int s;
            exam((int)words.IDEN);
            exam('=');
            s = (lex == '-') ? -1 : 1;
            if (lex == '+' || lex == '-')
            {
                get();
            }
            newob(nm, 1, s * lval);
            Console.WriteLine("Занесено об’єкт");
            exam((int)words.NUMB);
        }

        void dvarb()
        {
            //Console.WriteLine("dvarb");
            do
            {
                get();
                newob(TNM[lval], (ut==1? 2 : 3), (ut==1 ? cgv++ : ++clv));
                exam((int)words.IDEN);
            } while (lex == ',');
            exam(';');
        }

        void dfunc()
        {
            //Console.WriteLine("dfunc");
            int cp;
            get();
            cp = param();
            body();

        }

        int param()
        {
            //Console.WriteLine("param");
            int p;
            int cp = 0;
            exam('(');
            if (lex != ')')
            {
                newob(TNM[lval], 3, ++cp);
                exam((int)words.IDEN);
                while (lex == ',')
                {
                    get();
                    newob(TNM[lval], 3, ++cp);
                    exam((int)words.IDEN);
                }
            }
            exam(')');

            for (p = ptol; p < pto; p++)
            {
                TOB[p].val -= cp + 3;
            }
                
            return cp;

        }

        void body()
        {
            //Console.WriteLine("body");
            exam((int)words.BEGINL);
            while (lex == (int)words.INTL || lex == (int)words.CONSTL)
            {
                if (lex == (int)words.INTL)
                {
                    dvarb();
                }
                else
                {
                    dconst();
                }
            }
            stml();
            exam((int)words.ENDL);
        }

        void stml()
        {
            //Console.WriteLine("stml");
            stat();
            while (lex == ';')
            {
                get();
                stat();
            }
        }

        void stat()
        {
            //Console.WriteLine("stat");
            switch (lex)
            {
                case (int)words.IDEN:
                    get();
                    exam('=');
                    expr();
                    break;
                case (int)words.READL:
                    get();
                    exam((int)words.IDEN);
                    break;
                case (int)words.PRITL:
                    get();
                    expr();
                    break;
                case (int)words.RETRL:
                    get();
                    expr();
                    break;
                case (int)words.IFL:
                    get();
                    expr();
                    exam((int)words.THENL);
                    stml();
                    exam((int)words.ENDL);
                    break;
                case (int)words.WHILEL:
                    get();
                    expr();
                    exam((int)words.DOL);
                    stml();
                    exam((int)words.ENDL);
                    break;
                default:
                    Console.WriteLine("Синтакс.помилка в stat в nst =" + nst);
                    sw.WriteLine("Синтакс.помилка в stat в nst =" + nst);
                    break;
            }
        }

        void expr()
        {
            //Console.WriteLine("expr");
            if (lex == '+' || lex == '-')
            {
                get();
            }
            term();
            while (lex == '+' || lex == '-')
            {
                get();
                term();
            }
        }

        void term()
        {
            //Console.WriteLine("term");
            fact();
            while (lex == '*' || lex == '/' || lex == '%')
            {
                get();
                fact();
            }
        }

        void fact()
        {
            //Console.WriteLine("fact");
            switch (lex)
            {
                case '(':
                    get();
                    expr();
                    exam(')');
                    break;
                case (int)words.IDEN:
                    get();
                    if (lex == '(' )
                    {
                        get();
                        if (lex != ')')
                        {
                            fctl();
                        }                  
                        exam(')');
                    }
                    break;
                case (int)words.NUMB:
                    get();
                    break;
                default:
                    Console.WriteLine("Помилка  в FACT");
                    sw.WriteLine("Помилка  в FACT");
                    break;
            }
        }

        void fctl()
        {
            //Console.WriteLine("fctl");
            expr();
            while (lex == ',')
            {
                get();
                expr();
            }
        }

        void printTOB()
        {
            for(int i=0; i<pto; i++)
            {
                Console.WriteLine("name: " + TOB[i].name + ", what: " + TOB[i].what + ", value: " + TOB[i].val);
                sw.WriteLine("name: " + TOB[i].name + ", what: " + TOB[i].what + ", value: " + TOB[i].val);
            }
        }
        static void Main(string[] args)
        {
            string  name1="f1.txt";
            string  name2="f2.txt";
            Program ob = new Program(name1, name2);
            try
            {
                ob.get();
                ob.prog();
                ob.printTOB();
            }
            finally
            {
                if (ob.sr != null)
                    ob.sr.Dispose();
                if (ob.sw != null)
                    ob.sw.Dispose();
            }
            Console.ReadKey();
        }
    }
}