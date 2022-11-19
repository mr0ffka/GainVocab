using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Exceptions
{
    public class AuthException : ApplicationException
    {
        public const string InvalidCode = "Invalid code";
        public const string UserAlreadyExist = "User already exist";
        public const string EmailNotConfirmed = "Email address is not confirmed!";

        public AuthException(string message) : base(message)
        {

        }
    }
}
