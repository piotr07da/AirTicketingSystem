using Ats.Domain;

namespace Ats.Tests.TestTools
{
    public class FakeTenant : ITenant
    {
        private static TenantGroup _group;

        public TenantGroup Group => _group;

        public static void SetTenantGroup(TenantGroup group)
        {
            _group = group;
        }
    }
}
