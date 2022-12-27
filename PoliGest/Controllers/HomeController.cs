using PoliGest.Models;
using System;
using System.Collections.Generic;
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

    }
}