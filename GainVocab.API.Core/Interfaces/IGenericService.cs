using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GainVocab.API.Core.Models;
using GainVocab.API.Core.Models.Pager;

namespace GainVocab.API.Core.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        abstract int GetCount();
    }
}