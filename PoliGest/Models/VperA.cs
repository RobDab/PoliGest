using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoliGest.Models
{
    public class VperA
    {
        [Display(Name = "Tot Verbali")]
        public int TotVerbali { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }


    }
}