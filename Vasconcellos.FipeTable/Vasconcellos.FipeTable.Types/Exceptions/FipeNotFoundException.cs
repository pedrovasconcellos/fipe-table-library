using System;

namespace Vasconcellos.FipeTable.Types.Exceptions
{
    public class FipeNotFoundException : FipeException
    {
        public FipeNotFoundException()
        {
        }

        public FipeNotFoundException(string message)
            : base(message)
        {
        }

        public FipeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
