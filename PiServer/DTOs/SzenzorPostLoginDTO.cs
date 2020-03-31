using PiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiServer.DTOs
{
    public class SzenzorPostLoginDTO
    {
        public string Id { get; set; }
        public SzenzorTipus Tipus { get; set; }
    }
}
