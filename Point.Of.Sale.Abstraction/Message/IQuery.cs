using MediatR;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Abstraction.Message;

public interface IQuery<TResponse> : IRequest<IFluentResults<TResponse>>
{
}
