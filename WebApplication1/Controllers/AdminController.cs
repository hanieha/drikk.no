using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BLL;
using WebApplication1.BLL;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
  public class AdminController : Controller
  {
    // GET: Admin
    public ActionResult AdminSide()
    {
      if (Session["InnloggetAdmin"] == null)
      {
        Session["InnLoggetAdmin"] = false;
        ViewBag.Innlogget = false;
      }
      else
      {
        //return RedirectToAction("AdminSide", "Admin");
        ViewBag.Innlogget = (bool)Session["InnLoggetAdmin"];
      }
      return View();
    }

    [HttpPost]
    public ActionResult AdminSide(Kunde innLogget)
    {
      // sjekk om innlogging OK
      if (Admin_i_db(innLogget))
      {
        // ja brukernavn og passord er OK!
        Session["InnLoggetAdmin"] = true;
        ViewBag.Innlogget = true;
        return View();
      }
      else
      {
        // ja brukernavn og passord er OK!
        Session["InnLoggetAdmin"] = false;
        ViewBag.Innlogget = false;
        return RedirectToAction("Registrer", "Admin");
      }
    }

    // Sjekker om admin finnes i databasen
    private static bool Admin_i_db(Kunde innAdmin)
    {
      var adminBll = new AdminBLL();
      return adminBll.Admin_i_db(innAdmin);
    }

    // Registrerer ny admin
    public ActionResult Registrer()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Registrer(Kunde nyAdmin)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        if (ModelState.IsValid)
        {
          var adminBll = new AdminBLL();
          bool insertOK = adminBll.settInnNyAdmin(nyAdmin);
          if (insertOK)
            return RedirectToAction("AdminSide", "Admin");
        }
        return View();
      }

      return RedirectToAction("AdminSide", "Admin");

    }

    //lister alle kunder
    public ActionResult hentAlleKunder()
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var adminBll = new AdminBLL();
        List<Kunde> alleKunder = adminBll.hentAlleKunder(2);
        return View(alleKunder);
      }

      return RedirectToAction("AdminSide", "Admin");


    }

    //lister alle Ansatter
    public ActionResult HentAlleAnsatter()
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var adminBll = new AdminBLL();
        List<Kunde> alleKunder = adminBll.hentAlleKunder(1);
        return View(alleKunder);
      }

      return RedirectToAction("AdminSide", "Admin");

    }

    // Viser detaljer om en kunde
    public ActionResult KundeDetaljer(int id)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var adminBll = new AdminBLL();
        Kunde enKunde = adminBll.hentEnKunde(id);
        return View(enKunde);
      }
      return RedirectToAction("AdminSide", "Admin");

    }


    //Henter informasjonen til en Kunde for endring
    public ActionResult EndreKunde(int id)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var adminBll = new AdminBLL();
        Kunde endreKunde = adminBll.hentEnKunde(id);
        return View(endreKunde);
      }
      return RedirectToAction("AdminSide", "Admin");
    }

    //Endrer informasjonen til en Kunde
    [HttpPost]
    public ActionResult EndreKunde(int id, Kunde endreKunde)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var adminBll = new AdminBLL();
        int endringOK = adminBll.endreKunde(id, endreKunde);
        if (endringOK == 1)
        {
          return RedirectToAction("hentAlleAnsatter");
        }
        else if (endringOK == 2)
        {
          return RedirectToAction("hentAlleKunder");
        }
        return View();
      }
      return RedirectToAction("AdminSide", "Admin");
    }

    // Henter en Kunde fra databasen for å slette
    public ActionResult slettKunde(int id)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var adminBll = new AdminBLL();
        Kunde slettKunde = adminBll.hentEnKunde(id);
        return View(slettKunde);
      }
      return RedirectToAction("AdminSide", "Admin");
    }

    // sletter en Kunde fra databasen
    [HttpPost]
    public ActionResult slettKunde(int id, Kunde slettKunde)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var adminBll = new AdminBLL();
        bool slettOK = adminBll.slettKunde(id);
        if (slettOK)
        {
          return RedirectToAction("hentAlleKunder");
        }
        return View();
      }
      return RedirectToAction("AdminSide", "Admin");
    }

    // Logger ut
    public ActionResult LoggUt()
    {
      Session["InnLoggetAdmin"] = false;
      ViewBag.Innlogget = false;
      return RedirectToAction("Index", "Salg");
    }

    // Henter alle varer fra databasen
    public ActionResult AlleVarer()
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var vareBll = new VareBLL();
        List<Vare> alleVarer = vareBll.hentAlleVarer();
        return View(alleVarer);
      }
      return RedirectToAction("AdminSide", "Admin");

    }

    public ActionResult EndreVare(int id)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        List<SelectListItem> items = new List<SelectListItem>();

        var varebll = new VareBLL();
        var kategoriliste = varebll.hentAllKategorier();

        Vare endreVare = varebll.hentEnVare(id);
        ViewBag.katgorier = new SelectList(kategoriliste, "KatId", "KatNavn", endreVare.KatId);

        var landliste = varebll.HentAllLand();
        ViewBag.Lander = new SelectList(landliste, "LandId", "Navn");


        return View(endreVare);
      }
      return RedirectToAction("AdminSide", "Admin");
    }

    //Endrer informasjonen til en Kunde
    [HttpPost]
    public ActionResult EndreVare(int id, Vare endreVare)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var varebll = new VareBLL();
        bool endringOK = varebll.endreVare(id, endreVare);
        if (endringOK)
        {
          return RedirectToAction("AlleVarer");
        }

        return View();
      }
      return RedirectToAction("AdminSide", "Admin");
    }
    // Henter et vare fra databasen for å slette
    public ActionResult SlettVare(int id)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var varebll = new VareBLL();
        Vare slettVare = varebll.hentEnVare(id);
        return View(slettVare);
      }
      return RedirectToAction("AdminSide", "Admin");
    }

    // sletter en Kunde fra databasen
    [HttpPost]
    public ActionResult SlettVare(int id, Vare slettVare)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var varebll = new VareBLL();

        bool slettOK = varebll.slettVare(id);
        if (slettOK)
        {
          return RedirectToAction("AlleVarer");
        }
        return View();
      }
      return RedirectToAction("AdminSide", "Admin");
    }

    public ActionResult SettInnNyVare()
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        List<SelectListItem> items = new List<SelectListItem>();
        List<SelectListItem> Landitems = new List<SelectListItem>();
        var varebll = new VareBLL();
        var kategoriliste = varebll.hentAllKategorier();
        ViewBag.katgorier = new SelectList(kategoriliste, "KatId", "KatNavn");
        var landliste = varebll.HentAllLand();
        ViewBag.Lander = new SelectList(landliste, "LandId", "Navn");
        return View();
      }
      return RedirectToAction("AdminSide", "Admin");

    }

    [HttpPost]
    public ActionResult SettInnNyVare(Vare nyVare)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        if (ModelState.IsValid)
        {
          var vareBll = new VareBLL();
          bool insertOK = vareBll.settInnNyVare(nyVare);
          if (insertOK)
          {
            return RedirectToAction("AlleVarer", "Admin");
          }
        }
        return View();
      }
      return RedirectToAction("AdminSide", "Admin");

    }

    public ActionResult VareDetalj(int id)
    {
      if (Session["InnLoggetAdmin"] != null && ((bool)Session["InnLoggetAdmin"]))
      {
        var varebll = new VareBLL();
        Vare funnetVare = varebll.hentEnVare(id);
        return View(funnetVare);
      }
      return RedirectToAction("AdminSide", "Admin");
    }

  }
}