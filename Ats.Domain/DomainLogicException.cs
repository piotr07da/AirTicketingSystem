using System;

namespace Ats.Domain
{
    public class DomainLogicException : Exception
    {
        public DomainLogicException(string message)
            : base(message)
        {
        }
    }
}
