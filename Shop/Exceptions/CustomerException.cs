
using System;

namespace Shop.Exceptions
{
    public class CustomerException : Exception
    {
        public CustomerException(string message)
            : base(message)
        {
        }
    }
}