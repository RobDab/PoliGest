using PoliGest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliGest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Anagrafiche()
        {
            List<Anagrafica> list = new List<Anagrafica>();
            try
            {
                list = Anagrafica.GetAnagrafiche();
                
            }catch(Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }

            return View(list);

        }

        
        public ActionResult CreateAnagrafica()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAnagrafica(Anagrafica current)
        {
            
            try
            {
                ConnControl.InsertIntoTab("Anagrafica", current);
                return RedirectToAction("Anagrafiche");
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View();
            }
            
        }

        public ActionResult Verbali()
        {
            List<Verbale> list = new List<Verbale>();
            try
            {
                list = Verbale.GetVerbali();
            }catch(Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(list);
        }

        public ActionResult NewVerbale(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewVerbale(Verbale current, int id)
        {
            current.IDAnagrafica = id;
            try
            {
                ConnControl.InsertIntoTab("Verbale", current);
                return RedirectToAction("Verbali");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View();
            }
            
        }

        public ActionResult VerbaliPerAnagrafica()
        {
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();

            string cmdText = "SELECT COUNT(*) as totVerbali, Nome, Cognome FROM VerbaliTab INNER JOIN AnagraficheTab ON VerbaliTab.IDAnagrafica = AnagraficheTab.IDAnagrafica GROUP BY Nome, Cognome";
            List<VperA> recordList = new List<VperA>();
            try
            {
                SqlDataReader reader = ConnControl.Reader(cmdText, con);
                
                while(reader.Read())
                {
                    VperA current = new VperA()
                    {
                        TotVerbali = Convert.ToInt32(reader["totVerbali"]),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };

                    recordList.Add(current);
                    
                }
            }catch(Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }

            con.Close();
            return PartialView("_VerbaliPerAnagrafica",recordList);


        }

        public ActionResult DecurtamentoPerAnagrafica()
        {
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();

            List<TotDecurtamento> recordList = new List<TotDecurtamento>();
            string cmdText = "SELECT SUM(PuntiDecurtati) as totDecurtamento, Nome, Cognome  FROM VerbaliTab INNER JOIN AnagraficheTab ON VerbaliTab.IDAnagrafica = AnagraficheTab.IDAnagrafica GROUP BY Nome, Cognome";

            try
            {
                SqlDataReader reader = ConnControl.Reader(cmdText, con);

                while (reader.Read())
                {
                    TotDecurtamento current = new TotDecurtamento()
                    {
                        Totale = Convert.ToInt32(reader["totDecurtamento"]),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };

                    recordList.Add(current);

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }

            con.Close();
            return PartialView("_DecurtamentoPerAnagrafica", recordList);
        }

        public ActionResult DecurtamentiGravi()
        {
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();

            List<DecurtamentoGrave> recordList = new List<DecurtamentoGrave>();
            string cmdText = "SELECT Importo, Cognome, Nome, DataViolazione, PuntiDecurtati FROM VerbaliTab INNER JOIN AnagraficheTab On VerbaliTab.IDAnagrafica = AnagraficheTab.IDAnagrafica WHERE PuntiDecurtati > 10";

            try
            {
                SqlDataReader reader = ConnControl.Reader(cmdText, con);

                while (reader.Read())
                {
                    DecurtamentoGrave current = new DecurtamentoGrave()
                    {
                        Importo = Convert.ToDouble(reader["Importo"]),
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        DataViolazione = Convert.ToDateTime(reader["DataViolazione"]).ToString("d"),
                        PuntiDecurtati = Convert.ToInt32(reader["PuntiDecurtati"])
                    };

                    recordList.Add(current);

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }

            con.Close();
            return PartialView("_DecurtamentiGravi", recordList);
        }

        public ActionResult ImportiElevati()
        {
            SqlConnection con = ConnControl.ConnectDB();
            con.Open();

            List<HighFee> recordList = new List<HighFee>();
            string cmdText = "SELECT * FROM VerbaliTab WHERE Importo >= 400";

            try
            {
                SqlDataReader reader = ConnControl.Reader(cmdText, con);

                while (reader.Read())
                {
                    HighFee current = new HighFee()
                    {
                        IDVerbale = Convert.ToInt32(reader["IDVerbale"]),
                        Importo = Convert.ToDouble(reader["Importo"]),
                        DataViolazione = Convert.ToDateTime(reader["DataViolazione"]).ToString("d"),
                        PuntiDecurtati = Convert.ToInt32(reader["PuntiDecurtati"]),
                        IDAnagrafica = Convert.ToInt32(reader["IDAnagrafica"])
                    };

                    recordList.Add(current);

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }

            con.Close();
            return PartialView("_ImportiElevati", recordList);
        }

    }
}