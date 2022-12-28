using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoliGest.Models
{
    public class HighFee
    {
        public int IDVerbale { get; set; }

        public double Importo { get; set; }

        public string DataViolazione { get; set; }

        public int PuntiDecurtati { get; set; }

        public int IDAnagrafica { get; set; }
    }
}