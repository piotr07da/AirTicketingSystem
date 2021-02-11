using Ats.Core.Domain;
using System.Text.Json;

namespace Ats.Tests.TestTools
{
    public static class EventSerializer
    {
        public static string Serialize(IEvent e) => JsonSerializer.Serialize(e, e.GetType(), new JsonSerializerOptions() { });
    }
}
