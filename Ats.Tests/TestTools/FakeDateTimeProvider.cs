using Ats.Domain;
using System;

namespace Ats.Tests.TestTools
{
    public class FakeDateTimeProvider : IDateTimeProvider
    {
        private static DateTime _fakeUtcNow;

        public static void SetFakeUtcNow(DateTime fakeUtcNow)
        {
            _fakeUtcNow = fakeUtcNow;
        }

        public DateTime UtcNow => _fakeUtcNow;
    }
}
