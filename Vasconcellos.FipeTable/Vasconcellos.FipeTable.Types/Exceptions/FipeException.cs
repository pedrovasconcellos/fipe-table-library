using System;

namespace Vasconcellos.FipeTable.Types.Exceptions
{
    public class FipeException : Exception
    {
        public FipeException()
        {
        }

        public FipeException(string message)
            : base(message)
        {
        }

        public FipeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
