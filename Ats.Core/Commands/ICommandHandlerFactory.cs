namespace Ats.Core.Commands
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> Create<TCommand>()
            where TCommand : ICommand;
    }
}
