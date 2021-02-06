using System;

namespace Ats.Tests.TestTools
{
    public class ExceptionExpectation
    {
        public ExceptionExpectation(Type exceptionType, string exceptionMessage)
        {
            ExceptionType = exceptionType;
            ExceptionMessage = exceptionMessage;
        }

        public Type ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
