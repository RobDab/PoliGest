using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoliGest.Models
{
    public class DecurtamentoGrave
    {
        public double Importo { get; set; }

        public string Cognome { get; set; }

        public string Nome { get; set; }

        public string DataViolazione { get; set; }

        public int PuntiDecurtati { get; set; }
    }
}