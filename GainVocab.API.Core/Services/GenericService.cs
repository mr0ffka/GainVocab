using AutoMapper;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Data;

namespace GainVocab.API.Core.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly DefaultDbContext Context;
        protected readonly IMapper Mapper;

        public GenericService(DefaultDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }
    }
}