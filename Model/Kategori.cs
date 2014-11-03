using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Model
{
  public class Kategori
  {
    [Key]
    public int KatId { get; set; }
    public string KatNavn { get; set; }
    public List<Kategori> Kategorier { get; set; }
  }
}

