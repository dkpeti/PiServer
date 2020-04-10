using PiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiServer.DTOs
{
    public class SzenzorDTO
    {
        public long Id { get; set; }

        public string Nev { get; set; }

        public SzenzorTipus Tipus { get; set; }

        public string Megjegyzes { get; set; }
    }
}
