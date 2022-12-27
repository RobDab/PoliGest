using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoliGest.Models
{
    public class Verbale
    {
        public int IDVerbale { get; set; }

        public decimal Importo { get; set; }

        public string DataViolazione { get; set; }

        public int PuntiDecurtati { get; set; }

        public string Comune { get; set; }

        public int IDAnagrafica { get; set; }


        //static method to get all the Records from VerbaliTab
        public static List<Verbale> GetVerbali()
        {
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();

            string cmdText = "SELECT * FROM VerbaliTab";

            SqlDataReader reader = ConnControl.Reader(cmdText, con);
            List<Verbale> VerbaliList = new List<Verbale>();

            while(reader.Read())
            {
                Verbale current = new Verbale()
                {
                    Importo = Convert.ToDecimal(reader["Importo"]),
                    DataViolazione = Convert.ToDateTime(reader["dataViolazione"]).ToString("d"),
                    PuntiDecurtati = Convert.ToInt32(reader["PuntiDecurtati"]),
                    Comune = reader["ComuneViolazione"].ToString(),
                    IDAnagrafica = Convert.ToInt32(reader["IDAnagrafica"])
                };
                VerbaliList.Add(current);
            }

            con.Close();
            return VerbaliList;
        }
    }
}