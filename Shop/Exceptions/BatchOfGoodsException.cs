
using System;

namespace Shop.Exceptions
{
    public class BatchOfGoodsException : Exception
    {
        public BatchOfGoodsException(string message)
            : base(message)
        {
        }
    }
}