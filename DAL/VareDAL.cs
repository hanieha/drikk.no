using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DAL;
using WebApplication1.Model;

namespace DAL
{
  public class VareDAL
  {
    // Henter alle varer fra databasen
    public List<Vare> hentAlleVarer()
    {
      var db = new DrikkContext();

      var alleVarer = from v in db.Varer
                      select v;
      List<Vare> Varerist = new List<Vare>();
      foreach (var k in alleVarer)
      {

        Varerist.Add(new Vare
        {
          VareId = k.VareId,
          Navn = k.Navn,
          Pris = k.Pris,
          Antall = k.Antall,
          Land = db.Lander.FirstOrDefault(l => l.LandId == k.LandId).Navn,
          Kategori = db.Kategorier.FirstOrDefault(kat => kat.KatId == k.KatId).KatNavn
        });
      }
      return Varerist;
    }

    public Vare hentEnVare(int id)
    {
      var db = new DrikkContext();
      {
        var dbVare = db.Varer.Find(id);
        if (dbVare != null)
        {
          var enKategori = db.Kategorier.FirstOrDefault(k => k.KatId == dbVare.Kategori.KatId);
          dbVare.Kategori.KatNavn = enKategori.KatNavn;

          //var land = db.Lander.FirstOrDefault(l => l.Navn == dbVare.Land.Navn);
          var land = db.Lander.FirstOrDefault(l => l.LandId == dbVare.Land.LandId);
          dbVare.Land.Navn = land.Navn;

          var utVare = new Vare()
          {
            Navn = dbVare.Navn,
            Pris = dbVare.Pris,
            Kategori = dbVare.Kategori.KatNavn,
            Antall = dbVare.Antall,
            VareArtUrl = dbVare.VareArtUrl,
            Land = dbVare.Land.Navn
          };
          return utVare;
        }
        else
          return null;
      }
    }

    // Endrer informasjonen til en vare
    public bool endreVare(int id, Vare innVare)
    {
      var db = new DrikkContext();
      try
      {
        var endreVare = db.Varer.FirstOrDefault(v => v.VareId == id);
        endreVare.Navn = innVare.Navn;
        endreVare.Pris = innVare.Pris;
        endreVare.Antall = innVare.Antall;
        endreVare.VareArtUrl = innVare.VareArtUrl;
        endreVare.KatId = innVare.KatId;
        endreVare.LandId = innVare.LandId;
        db.SaveChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public List<Kategori> hentAllKategorier()
    {
      var db = new DrikkContext();
      try
      {
        var katList = from k in db.Kategorier
                      select k;
        List<Kategori> kategoriList = new List<Kategori>();
        foreach (var k in katList)
        {
          kategoriList.Add(new Kategori() { KatId = k.KatId, KatNavn = k.KatNavn });
        }

        return kategoriList;

      }
      catch
      {
        return null;
      }
    }

    public List<Land> HentAllLand()
    {
      var db = new DrikkContext();
      try
      {
        var landerList = from k in db.Lander
                         select k;
        List<Land> landList = new List<Land>();
        foreach (var k in landerList)
        {
          landList.Add(new Land() { LandId = k.LandId, Navn = k.Navn });
        }

        return landList;

      }
      catch
      {
        return null;
      }
    }

    public bool slettVare(int id)
    {
      var db = new DrikkContext();
      try
      {
        var slettVare = db.Varer.Find(id);
        db.Varer.Remove(slettVare);
        db.SaveChanges();
        return true;
      }
      catch (Exception feil)
      {
        return false;
      }
    }


    public bool settInnNyVare(Vare innVare)
    {
      var nyVare = new Varer()
      {
        Navn = innVare.Navn,
        LandId = innVare.LandId,
        Pris = innVare.Pris,
        Antall = innVare.Antall,
        KatId = innVare.KatId,
        VareArtUrl = innVare.VareArtUrl
      };
      var db = new DrikkContext();
      try
      {

        db.Varer.Add(nyVare);
        db.SaveChanges();
        return true;
      }
      catch (Exception feil)
      {
        return false;
      }
    }



  }
}
