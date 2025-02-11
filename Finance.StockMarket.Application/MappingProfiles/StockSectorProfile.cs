using AutoMapper;
using Finance.StockMarket.Application.Features.StockSector.Queries.GetAllStockSectors;
using Finance.StockMarket.Application.Features.StockSector.Queries.GetStockSectorDetails;
using Finance.StockMarket.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.MappingProfiles
{
    class StockSectorProfile : Profile
    {
        public StockSectorProfile()
        {
            CreateMap<StockSectorDTO, StockSector>().ReverseMap();
            CreateMap<StockSector, StockSectorDetailDTO>();
        }
    }
}
