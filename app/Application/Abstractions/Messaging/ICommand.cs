using ErrorOr;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface ICommandBase
{
}

public interface ICommand : IRequest<ErrorOr<Success>>, ICommandBase
{
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>, ICommandBase
    where TResponse : class
{
}
