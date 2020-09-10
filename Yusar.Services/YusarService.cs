using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yusar.Core;

namespace Yusar.Services
{
    public class YusarService : IYusarService
    {
        private readonly YusarRepository _repository;

        public YusarService(YusarRepository repository)
        {
            _repository = repository;
        }


    }
}
