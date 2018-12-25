using System;

namespace Lab4.Exceptions
{
    public class ExpectedStatementException : Exception
    {
        private string message = "Identify Statement Exception";

        public override string Message
        {
            get { return message; }
        }
    }
}