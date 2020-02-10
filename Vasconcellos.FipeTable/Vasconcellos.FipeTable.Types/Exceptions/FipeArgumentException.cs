using System;
namespace Vasconcellos.FipeTable.Types.Exceptions
{
    public class FipeArgumentException : FipeException
    {
        public FipeArgumentException()
        {
        }

        public FipeArgumentException(string message)
            : base(message)
        {
        }

        public FipeArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
