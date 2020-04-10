using PiServer.Context;
using PiServer.DTOs;
using PiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PiServer.DataManagers
{
    public class SzenzorManager : ISzenzorManager
    {
        private readonly PiDbContext _piDbContext;
        private readonly IrrigationServerConnection _irrigationServerConnection;

        public SzenzorManager(PiDbContext context, IrrigationServerConnection irrigationServerConnection)
        {
            _piDbContext = context;
            _irrigationServerConnection = irrigationServerConnection;
        }

        public bool Login(string id, SzenzorTipus tipus, IPAddress ipAddress)
        {
            Szenzor szenzor = _piDbContext.Szenzorok
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if(szenzor == null)
            {
                var (success, szenzorId) = _irrigationServerConnection.RegisterSzenzor(id, tipus).Result;
                if(success)
                {
                    szenzor = new Szenzor()
                    {
                        Id = id,
                        IP = ipAddress.ToString(),
                        RemoteId = szenzorId,
                        Tipus = tipus
                    };
                    _piDbContext.Szenzorok.Add(szenzor);
                    _piDbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                szenzor.IP = ipAddress.ToString();
                _piDbContext.SaveChanges();
                return true;
            }
        }

        public bool PostMeresData(string id, long meresData)
        {
            Szenzor szenzor = _piDbContext.Szenzorok
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if(szenzor == null)
            {
                return false;
            }
            else
            {
                return _irrigationServerConnection.PostMeresData(szenzor.RemoteId, meresData).Result;
            }
        }
    }
}
