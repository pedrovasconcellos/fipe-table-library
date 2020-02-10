using System;

namespace Vasconcellos.FipeTable.DownloadService.Exceptions
{
    public class FipeNotFoundException : Exception
    {
        public FipeNotFoundException()
        {
        }

        public FipeNotFoundException(string message)
            : base(message)
        {
        }

        public FipeNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
