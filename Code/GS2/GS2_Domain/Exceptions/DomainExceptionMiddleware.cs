using System;
using System.Runtime.Serialization;

namespace GS2_Domain.Exceptions
{
    [Serializable]
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
        protected DomainException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}