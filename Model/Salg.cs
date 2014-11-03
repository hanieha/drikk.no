﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Model
{
  public class Salg
  {
    [Key]
    public int bid { get; set; }
    public decimal Belop { get; set; }
    public System.DateTime OrderDate { get; set; }
    public virtual List<Salg> Bestillinger { get; set; }
  }
}