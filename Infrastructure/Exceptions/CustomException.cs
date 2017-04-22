using System;

namespace Infrastructure.Exceptions
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException()
        {

        }

        public CustomException(string message) : base(message)
        {

        }

    }
}
