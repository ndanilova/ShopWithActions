using System;

namespace Shop.Exceptions
{
    public class SupplyDataException : Exception
    {
        public SupplyDataException(string message)
            : base(message)
        {
        }
    }
}