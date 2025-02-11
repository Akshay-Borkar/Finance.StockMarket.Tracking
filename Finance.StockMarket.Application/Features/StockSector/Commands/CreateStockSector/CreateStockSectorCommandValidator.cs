using Finance.StockMarket.Application.Contracts.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Commands.CreateStockSector
{
    public class CreateStockSectorCommandValidator: AbstractValidator<CreateStockSectorCommand>
    {
        private readonly IStockSectorRepository _stockSectorRepository;

        public CreateStockSectorCommandValidator(IStockSectorRepository stockSectorRepository)
        {
            RuleFor(p => p.StockSectorName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.SectorPE)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

            RuleFor(c => c)
                .MustAsync(StockSectorNameUnique).WithMessage("Stock Sector already exists.");

            this._stockSectorRepository = stockSectorRepository;
        }

        private Task<bool> StockSectorNameUnique(CreateStockSectorCommand command, CancellationToken token)
        {
            return _stockSectorRepository.IsUniqueStockSector(command.StockSectorName);
        }
    }
}
