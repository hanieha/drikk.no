using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using WebApplication1.Model;

namespace BLL
{
   public class VareBLL
  {
     public List<Vare> hentAlleVarer()
     {
       var vareDal = new VareDAL();
       List<Vare> alleVarer = vareDal.hentAlleVarer();
       return alleVarer;
     }

     //public bool settInnNyVare(Vare nyVare)
     //{
     //  var vareDal = new VareDAL();
     //  return vareDal.(nyVare);
     //}
     //Henter en vare
     public Vare hentEnVare(int id)
     {
       var vareDal = new VareDAL();
       return vareDal.hentEnVare(id);
     }

     // Endrer informasjonen til en vare
     public bool endreVare(int id, Vare innVare)
     {
       var vareDal = new VareDAL();
       return vareDal.endreVare(id, innVare);
     }
     public List<Kategori> hentAllKategorier()
     {
       var vareDal = new VareDAL();
       return vareDal.hentAllKategorier();
     }

     public List<Land> HentAllLand()
     {
       var vareDal = new VareDAL();
       return vareDal.HentAllLand();
     }

     public bool slettVare(int id)
     {
       var vareDal = new VareDAL();
       return vareDal.slettVare(id);
     }

     public bool settInnNyVare(Vare nyVare)
     {
       var vareDal = new VareDAL();
       return vareDal.settInnNyVare(nyVare);
     }
  }
}
