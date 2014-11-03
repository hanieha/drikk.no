using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Model;

namespace WebApplication1.DAL
{
  public class AdminDAL
  {

    public bool settInnNyAdmin(Kunde innKunde)
    {
      var nyKunde = new Kunder()
      {
        Fornavn = innKunde.Fornavn,
        Etternavn = innKunde.Etternavn,
        Adresse = innKunde.Adresse,
        Epost = innKunde.Epost,
        Postnr = innKunde.Postnr,
        Passord = lagHash(innKunde.Passord),
        Rolle = 1
      };
      var db = new DrikkContext();
      try
      {
        var eksistererPostnr = db.Poststeder.Find(innKunde.Postnr);
        if (eksistererPostnr == null)
        {
          var nyttPoststed = new Poststeder()
          {
            Postnr = innKunde.Postnr,
            Poststed = innKunde.Poststed
          };
          db.Poststeder.Add(nyttPoststed);
          nyKunde.Poststeder = nyttPoststed;
        }
        db.Kunder.Add(nyKunde);
        db.SaveChanges();
        return true;
      }
      catch (Exception feil)
      {
        return false;
      }

    }

    public bool Admin_i_db(Kunde innAdmin)
    {
      using (var db = new DrikkContext())
      {
        byte[] passordDB = lagHash(innAdmin.Passord);
        var funnetAdmin = db.Kunder.FirstOrDefault(b => b.Passord == passordDB && b.Epost == innAdmin.Epost);
        if (funnetAdmin == null)
        {
          return false;
        }
        else
        {
          return true;
        }
      }
    }


    public byte[] lagHash(string innPassord)
    {
      byte[] innData, utData;
      var algoritme = System.Security.Cryptography.SHA256.Create();
      innData = System.Text.Encoding.ASCII.GetBytes(innPassord);
      utData = algoritme.ComputeHash(innData);
      return utData;
    }

    //lister alle kunder
    public List<Kunde> hentAlleKunder(int rolle)
    {
      var db = new DrikkContext();
      var alleKunder = from k in db.Kunder
                       where k.Rolle == rolle
                       select k;

      List<Kunde> kunderList = new List<Kunde>();
      foreach (var k in alleKunder)
      {

        kunderList.Add(new Kunde
        {
          Kid = k.Kid,
          Fornavn = k.Fornavn,
          Etternavn = k.Etternavn,
          Epost = k.Epost,
          Adresse = k.Adresse,
          Postnr = k.Postnr,
          Poststed = db.Poststeder.FirstOrDefault(ps => ps.Postnr == k.Postnr).Poststed,
          Rolle = k.Rolle
        });
      }
      return kunderList;
    }

    // Henter info om en kunde
    public Kunde hentEnKunde(int id)
    {
      var db = new DrikkContext();
      var enDbKunde = db.Kunder.Find(id);
      if (enDbKunde == null)
      {
        return null;
      }
      else
      {
        var utKunde = new Kunde()
        {
          Kid = enDbKunde.Kid,
          Fornavn = enDbKunde.Fornavn,
          Etternavn = enDbKunde.Etternavn,
          Epost = enDbKunde.Epost,
          Adresse = enDbKunde.Adresse,
          Postnr = enDbKunde.Postnr,
          Poststed = db.Poststeder.FirstOrDefault(ps => ps.Postnr == enDbKunde.Postnr).Poststed
        };
        return utKunde;
      }
    }

    // Endrer info om en kunde
    public int endreKunde(int id, Kunde innKunde)
    {
      var db = new DrikkContext();
      try
      {
        var endreKunde = db.Kunder.FirstOrDefault(k => k.Kid == id);
        endreKunde.Fornavn = innKunde.Fornavn;
        endreKunde.Etternavn = innKunde.Etternavn;
        endreKunde.Adresse = innKunde.Adresse;
        endreKunde.Epost = innKunde.Epost;
        if (endreKunde.Postnr != innKunde.Postnr)
        {
          // Postnummeret er endret. Må først sjekke om det nye postnummeret eksisterer i tabellen.
          Poststeder eksisterendePoststed = db.Poststeder.FirstOrDefault(p => p.Postnr == innKunde.Postnr);
          if (eksisterendePoststed == null)
          {
            var nyttPoststed = new Poststeder()
            {
              Postnr = innKunde.Postnr,
              Poststed = innKunde.Poststed
            };
            db.Poststeder.Add(nyttPoststed);
            endreKunde.Postnr = nyttPoststed.Postnr;
          }
          else
          {   // poststedet med det nye postnr eksisterer, endre bare postnummeret til kunden
            endreKunde.Postnr = innKunde.Postnr;
          }
        };
        db.SaveChanges();
        return endreKunde.Rolle;
      }
      catch
      {
        return 0;
      }
    }

    // Fjerner en Kunde fra databasen
    public bool slettKunde(int id)
    {
      var db = new DrikkContext();
      // Kunde kunde = new Kunde();
      try
      {
        var slettKunde = db.Kunder.Find(id);
        db.Kunder.Remove(slettKunde);
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