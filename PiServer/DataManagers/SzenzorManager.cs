using PiServer.Context;
using PiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiServer.DataManagers
{
    public class SzenzorManager : ISzenzorManager
    {
        readonly PiDbContext _piDbContext;

        public SzenzorManager(PiDbContext context)
        {
            _piDbContext = context;
        }

        public bool Login(string id, SzenzorTipus tipus)
        {
            throw new NotImplementedException();
        }

        public bool PostMeresData(string id, long meresData)
        {
            throw new NotImplementedException();
        }
    }
}
