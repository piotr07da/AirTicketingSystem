using System.Threading.Tasks;

namespace Ats.Core.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync(ICommand command);
    }
}
