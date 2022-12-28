using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoliGest.Models
{
    public class ConnControl
    {
        public static SqlConnection ConnectDB()
        {
            string ConStringName = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            SqlConnection con = new SqlConnection(ConStringName);
            return con;
        }

        public static SqlDataReader Reader(string cmdText, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(cmdText, con);
            SqlDataReader reader = cmd.ExecuteReader();

            return reader;
        }

        // Possibile riutilizzare InsertIntoTab in diverse action per aggiungere sia Anagrafiche che Verbali 
        // specificando il tipo come parametro. 
        public static int InsertIntoTab(string model, Object current )   
        {
            
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            if (model == "Anagrafica")
            {
               Anagrafica anagrafica = current as Anagrafica;

                cmd.CommandText = "INSERT INTO AnagraficheTab VALUES (@Nome, @Cognome, @Indirizzo, @PuntiPatente, @NeoPatentato)";
                cmd.Parameters.AddWithValue("Nome", anagrafica.Nome);
                cmd.Parameters.AddWithValue("Cognome", anagrafica.Cognome);
                cmd.Parameters.AddWithValue("Indirizzo", anagrafica.Indirizzo);
                cmd.Parameters.AddWithValue("PuntiPatente", anagrafica.PuntiPatente);
                cmd.Parameters.AddWithValue("NeoPatentato", anagrafica.NeoPatentato);

            }
            else if(model == "Verbale")
            {
                Verbale verb = current as Verbale;

                cmd.CommandText = "INSERT INTO VerbaliTab VALUES (@Importo, @Data , @Punti, @Comune, @IDAnagrafica)";
                cmd.Parameters.AddWithValue("Importo", verb.Importo);
                cmd.Parameters.AddWithValue("Data", verb.DataViolazione);
                cmd.Parameters.AddWithValue("Punti", verb.PuntiDecurtati);
                cmd.Parameters.AddWithValue("Comune", verb.Comune);
                cmd.Parameters.AddWithValue("IDAnagrafica", verb.IDAnagrafica);

                if(verb.PuntiDecurtati > 0)
                {
                    RemovePoints(verb.IDAnagrafica, verb.PuntiDecurtati);
                }

            }
            else
            {
                return 0;
            }


            int rows = cmd.ExecuteNonQuery();
            con.Close();
            return rows;

        }


        // RemovePoints è una routine che viene richiamata in callback quando creando un verbale si dichiarano punti da rimuove dalla patente.
        public static void RemovePoints(int IDAnagrafica, int Points)
        {

            List<Anagrafica> listAnagraf = Anagrafica.GetAnagrafiche();
            int PointsToRemove = Points;
            
            // Cerca l'anagrafica e raddoppia i punti da rimuovere se si tratta di un neo-patentato
            foreach(Anagrafica a in listAnagraf)
            {
                if(a.IDAngrafica == IDAnagrafica)
                {
                    if (a.NeoPatentato)
                    {
                        PointsToRemove *= 2;
                    }
                }
            }

            
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.Parameters.AddWithValue("IDAnagrafica", IDAnagrafica);
            cmd.Parameters.AddWithValue("Punti", PointsToRemove);

            cmd.CommandText = "UPDATE AnagraficheTab SET PuntiPatente = PuntiPatente - @Punti WHERE IDAnagrafica = @IDAnagrafica";

            cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}