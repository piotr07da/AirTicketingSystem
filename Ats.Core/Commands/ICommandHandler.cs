using System.Threading.Tasks;

namespace Ats.Core.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
