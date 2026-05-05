using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.DeleteStock;

public record DeleteStockCommand(Guid StockId, Guid UserId) : IRequest;
