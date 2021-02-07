using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Ats.Core.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory _handlerFactory;

        private static readonly ConcurrentDictionary<Type, object> _commandHandlersCache = new ConcurrentDictionary<Type, object>();

        public CommandDispatcher(ICommandHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
        }

        public async Task DispatchAsync(ICommand command)
        {
            if (command is null) throw new ArgumentNullException(nameof(command));

            var commandType = command.GetType();

            if (!_commandHandlersCache.TryGetValue(commandType, out object handler))
            {
                handler = typeof(ICommandHandlerFactory).GetMethod(nameof(ICommandHandlerFactory.Create)).MakeGenericMethod(commandType).Invoke(_handlerFactory, new object[0]);
                if (handler == null)
                    throw new Exception($"Cannot find handler for command of type {commandType.Name}.");

                _commandHandlersCache.TryAdd(commandType, handler);
            }

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            var handleMethod = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync));

            try
            {
                var handleTask = handleMethod.Invoke(handler, new[] { command }) as Task;
                await handleTask;
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }
    }
}
