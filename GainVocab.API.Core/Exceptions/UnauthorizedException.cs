using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public const string InvalidCode = "Invalid code";
        public const string UserAlreadyExist = "User already exist";
        public const string EmailNotConfirmed = "Email address is not confirmed!";
        public const string Locked = "Account has been locked";

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
