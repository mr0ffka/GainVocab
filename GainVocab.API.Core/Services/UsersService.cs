using AutoMapper;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Repository;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Services
{
    public class UsersService : GenericService<APIUser>, IUsersSerivce
    {
        public UsersService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
