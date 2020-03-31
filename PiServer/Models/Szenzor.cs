using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PiServer.Models
{
    public enum SzenzorTipus
    {
        Homerseklet, Talajnedvesseg
    }
    public class Szenzor
    {
        [Key]
        public string Id { get; set; }

        public string IP { get; set; }

        public SzenzorTipus Tipus { get; set; }

        public long RemoteId { get; set; }
    }
}
