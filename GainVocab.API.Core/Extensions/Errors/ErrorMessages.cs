using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Extensions.Errors
{
    public static class ErrorMessages
    {
        public static string UnauthorizedMessage_EmailNotConfirmed = "User cannot sign in without a confirmed email";
        public static string UnauthorizedMessage_IncorrectCredentials = "Incorrect credentials";
        public static string UnauthorizedMessage_AccountLocked = "Account locked";
    }
}
