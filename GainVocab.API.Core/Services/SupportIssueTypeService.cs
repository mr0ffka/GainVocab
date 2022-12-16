using AutoMapper;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Data.Models;
using GainVocab.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Models.SupportIssue;
using GainVocab.API.Core.Models.Users;

namespace GainVocab.API.Core.Services
{
    public class SupportIssueTypeService : GenericService<SupportIssueType>, ISupportIssueTypeService
    {

        public SupportIssueTypeService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public SupportIssueType Get(string publicId)
        {
            var entity = Context.SupportIssueType.FirstOrDefault(t => t.PublicId == publicId);
            if (entity == null)
                throw new NotFoundException("SupportIssueType", publicId);

            return entity;
        }

        public SupportIssueType Get(long id)
        {
            var entity = Context.SupportIssueType.FirstOrDefault(t => t.Id == id);
            if (entity == null)
                throw new NotFoundException("SupportIssueType: id", id);

            return entity;
        }

        public List<TypeItemModel> GetOptionsList()
        {
            var query = Context.SupportIssueType.ToList();
            var result = new List<TypeItemModel>();

            foreach (var type in query)
            {
                var temp = new TypeItemModel();
                temp.PublicId = type.PublicId;
                temp.Name = type.Name;
                result.Add(temp);
            }

            return result;
        }
    }
}
