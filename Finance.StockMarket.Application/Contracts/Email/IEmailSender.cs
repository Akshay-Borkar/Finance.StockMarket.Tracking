using Finance.StockMarket.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(EmailMessage emailMessage);
    }
}
