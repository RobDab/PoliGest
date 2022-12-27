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
                Verbale verbale = current as Verbale;
                cmd.CommandText = "INSERT INTO VerbaliTab VALUES (@Importo, @Data , @Punti, @Comune, @IDAnagrafica)";
                cmd.Parameters.AddWithValue("Importo", verbale.Importo);
                cmd.Parameters.AddWithValue("Data", verbale.DataViolazione);
                cmd.Parameters.AddWithValue("Punti", verbale.PuntiDecurtati);
                cmd.Parameters.AddWithValue("Comune", verbale.Comune);
                cmd.Parameters.AddWithValue("IDAnagrafica", verbale.IDAnagrafica);

            }
            else
            {
                return 0;
            }
            

           
            int rows = cmd.ExecuteNonQuery();
            return rows;

        }
    }
}