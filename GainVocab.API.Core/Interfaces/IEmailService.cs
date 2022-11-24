using GainVocab.API.Core.Models.Email;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationEmail(APIUser user, string token);
        Task SendForgotPasswordEmail(APIUser user, string token);
    }
}
