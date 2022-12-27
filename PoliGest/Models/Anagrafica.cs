using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoliGest.Models
{
    public class Anagrafica
    {
        public int IDAngrafica { get; set; }


        public string Nome { get; set;}

        public string Cognome { get; set; }

        public string Indirizzo { get; set; }

        public int PuntiPatente { get; set; }

        public bool NeoPatentato { get; set; }

        //Static method to get all the Records from AngraficheTab
        public static List<Anagrafica> GetAnagrafiche()
        {
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();
            string cmdText = "SELECT * FROM AnagraficheTab";

            //use cmdText as param in static method Reader() of ConnControl
            SqlDataReader reader = ConnControl.Reader(cmdText, con);
            List<Anagrafica> AnagraficheList = new List<Anagrafica>();
            while (reader.Read())
            {
                Anagrafica current = new Anagrafica()
                {
                    IDAngrafica = Convert.ToInt32(reader["IDAnagrafica"]),
                    Nome = reader["Nome"].ToString(),
                    Cognome = reader["Cognome"].ToString(),
                    Indirizzo = reader["Indirizzo"].ToString(),
                    PuntiPatente = Convert.ToInt32(reader["PuntiPatente"]),
                    NeoPatentato = Convert.ToBoolean(reader["NeoPatentato"])
                };
                AnagraficheList.Add(current);
            }
            con.Close();
            return (AnagraficheList);
        }   

    }
}