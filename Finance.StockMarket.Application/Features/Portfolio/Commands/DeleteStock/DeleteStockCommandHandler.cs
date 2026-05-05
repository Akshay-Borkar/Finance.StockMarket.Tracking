using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Exceptions;
using Finance.StockMarket.Domain;
using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.DeleteStock;

public class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand>
{
    private readonly IStockRepository _stockRepository;
    private readonly IInvestmentRepository _investmentRepository;

    public DeleteStockCommandHandler(IStockRepository stockRepository, IInvestmentRepository investmentRepository)
    {
        _stockRepository = stockRepository;
        _investmentRepository = investmentRepository;
    }

    public async Task Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByIdAsync(request.StockId);

        if (stock is null)
            throw new NotFoundException(nameof(Stock), request.StockId);

        if (stock.UserId != request.UserId)
            throw new BadRequestException("You do not have permission to delete this stock.");

        // Delete all investments for this stock first (cascade not enforced at app level)
        var investments = await _investmentRepository.GetInvestmentsByStockId(request.StockId);
        foreach (var investment in investments)
            await _investmentRepository.DeleteAsync(investment);

        await _stockRepository.DeleteAsync(stock);
    }
}
