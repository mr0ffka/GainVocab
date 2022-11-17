using AutoMapper;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;

namespace GainVocab.API.Core.Services
{
    public class UsersService : GenericService<APIUser>, IUsersSerivce
    {
        public UsersService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
