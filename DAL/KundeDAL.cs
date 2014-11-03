using WebApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.DAL
{
  public class KundeDAL
  {
   
    public bool Registrer(Kunde innKunde)
    {
      var nyKunde = new Kunder()
      {
        Fornavn = innKunde.Fornavn,
        Etternavn = innKunde.Etternavn,
        Adresse = innKunde.Adresse,
        Epost = innKunde.Epost,
        Postnr = innKunde.Postnr,
        Passord = lagHash(innKunde.Passord),
        Rolle = 2
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

    public byte[] lagHash(string innPassord)
    {
      byte[] innData, utData;
      var algoritme = System.Security.Cryptography.SHA256.Create();
      innData = System.Text.Encoding.ASCII.GetBytes(innPassord);
      utData = algoritme.ComputeHash(innData);
      return utData;
    }

    public bool Endre(int Id, Kunde ekunde)
    {
      using (var db = new DrikkContext())
      {
        try
        {
          var kun = db.Kunder.SingleOrDefault(k => k.Kid == Id);

          if (kun != null)
          {
            kun.Fornavn = ekunde.Fornavn;
            kun.Etternavn = ekunde.Etternavn;
            kun.Adresse = ekunde.Adresse;
            kun.Postnr = ekunde.Postnr;
            var eksistererPostnr = db.Poststeder.Find(ekunde.Postnr);
            if (eksistererPostnr == null)
            {
              var nyttPoststed = new Poststeder()
              {
                Postnr = ekunde.Postnr,
                Poststed = ekunde.Poststed
              };
              db.Poststeder.Add(nyttPoststed);
              db.SaveChanges();
              kun.Poststeder = nyttPoststed;
            }
            
            kun.Passord = lagHash(ekunde.Passord);
            db.SaveChanges();
            return true;
          }

          return false;
        }
        catch { return false; }
      }
    }

    // Henter informasjon om en kunde fra database
    public Kunde hentEnKunde(Kunde kun)
    {
      var db = new DrikkContext();
      byte[] p = lagHash(kun.Passord);
      var enDbKunde = db.Kunder.FirstOrDefault(k => k.Epost == kun.Epost && k.Passord == p);
      if (enDbKunde != null)
      {
        var kundeInfo = new Kunde()
        {
          Kid = enDbKunde.Kid,
          Fornavn = enDbKunde.Fornavn,
          Etternavn = enDbKunde.Etternavn,
          Adresse = enDbKunde.Adresse,
          Epost = enDbKunde.Epost,
          Postnr = enDbKunde.Postnr,
          Poststed = db.Poststeder.FirstOrDefault(ps => ps.Postnr == enDbKunde.Postnr).Poststed,
          Rolle = enDbKunde.Rolle
        };
        return kundeInfo;
      }
      else
        return null;
    }

    

  }
}
