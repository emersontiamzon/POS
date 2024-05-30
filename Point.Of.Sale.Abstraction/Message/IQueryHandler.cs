using MediatR;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Abstraction.Message;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, IFluentResults<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
