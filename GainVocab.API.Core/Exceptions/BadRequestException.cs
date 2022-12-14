using GainVocab.API.Core.Extensions.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<ErrorEntry> Errors { get; set; }

        public BadRequestException(string message) : base(message)
        {
            Errors = new List<ErrorEntry>();
            var error = new ErrorEntry
            {
                Code = HttpStatusCode.BadRequest.ToString(),
                Title = message,
                Source = ""
            };
            Errors.Add(error);
        }

        public BadRequestException(string message, List<ErrorEntry> errors) : base(message)
        {
            Errors = errors;
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
