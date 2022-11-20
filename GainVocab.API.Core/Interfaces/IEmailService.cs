using GainVocab.API.Core.Models.Email;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailSendModel request);

        Task SendEmailConfirmation(APIUser user);
    }
}
