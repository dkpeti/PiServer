using PiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiServer.DataManagers
{
    public interface ISzenzorManager
    {
        bool Login(string id, SzenzorTipus tipus);

        bool PostMeresData(string id, long meresData);
    }
}
