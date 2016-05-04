using System;

namespace StockExchange.Repository.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public string ExceptionMessage { get; set; }

        public NotFoundException(string message)
        {
            this.ExceptionMessage = message;
        }
    }
}
