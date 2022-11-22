using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GainVocab.API.Core.Extensions.Errors;

namespace GainVocab.API.Core.Interfaces
{
    public interface IExceptionHandler
    {
        public Task<ErrorResponse> HandleException(Exception exception);
    }
}
