using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Model;


namespace WebApplication1.DAL
{
  public class SalgDAL
  {

    public bool settInBestilling(Bestilling bestilt)
    {
      var db = new DrikkContext();

      using (var dbTransaksjon = db.Database.BeginTransaction())
      {
        var nybestilling = new Bestilling()
        {
          Bid = bestilt.Bid,
          //Antall = bestilt.Antall,
          Belop = bestilt.Belop,
          OrderDate = bestilt.OrderDate,
          //Kunder_Kid = bestilt.Kunder_Kid
        };

        try
        {
          db.Bestillinger.Add(bestilt);
          db.SaveChanges();
          dbTransaksjon.Commit();
          return true;
        }
        catch (Exception feil)
        {
          dbTransaksjon.Rollback();
          return false;
        }
      }
    }
    /* public Salg SisteSolgt(int bid)
     {
         var db = new DrikkContext();
         {
             var dbVare = db.Bestillinger.Find(bid);
             {
                 var bestiling = db.Bestillinger.FirstOrDefault(b => b.OrderDate == dbVare.OrderDate);
                 // dbVare.Kategori = Knavn;
                 var utBestilling = new Salg()
                 {
                     bid = dbVare.Bid,
                     Belop = dbVare.Belop
                 };
                 return utBestilling;
             }
         }
     }*/

    public Kategori KategoriListe(string kategori)
    {
      var db = new DrikkContext();
      var kat = db.Kategorier.Include("Kategorier").Single(g => g.KatNavn == kategori);
      if (kat != null)
      {
        var katListe = new Kategori()
        {
          KatId = kat.KatId,
          KatNavn = kat.KatNavn
        };

        return katListe;
      }
      else
        return null;
    }

    public Vare Detaljer(int VareId)
    {
      var db = new DrikkContext();
      {
          var drikke = db.Varer.FirstOrDefault(v => v.VareId == VareId);
        if (drikke != null)
        {
          var drikkeInfo = new Vare()
          {
            VareId = drikke.VareId,
            Navn = drikke.Navn,
            Land = drikke.Land.Navn,
            Pris = drikke.Pris,
            VareArtUrl = drikke.VareArtUrl,
            Kategori = drikke.Kategori.KatNavn
          };
          //var lnd = db.Lander.FirstOrDefault(k => k.LandId == drikke.LandId);
          //drikke.Land.Navn = lnd.Navn;

          return drikkeInfo;
        }
        else
          return null;
      }
    }

    public Vare hentEnVare(int vareId)
    {
      var db = new DrikkContext();
      {
        var dbVare = db.Varer.FirstOrDefault(v => v.VareId == vareId);
        try
        {
          
          var utVare = new Vare()
           {
             VareId = dbVare.VareId,
             Navn = dbVare.Navn,
             Land = db.Lander.FirstOrDefault(l => l.LandId == dbVare.LandId).Navn,
             Pris = dbVare.Pris,
             VareArtUrl = dbVare.VareArtUrl
             
             //Kategori = db.Kategorier.FirstOrDefault(k => k.KatId == dbVare.KatId).KatNavn

           };

          return utVare;
        }
        catch (Exception ex)
        {
          return null;
        }

      }
    }


  }
}

