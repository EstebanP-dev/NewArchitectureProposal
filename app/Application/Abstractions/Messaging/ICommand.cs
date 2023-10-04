using ErrorOr;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface ICommand : IRequest<ErrorOr<Success>>
{
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>
    where TResponse : class
{
}
