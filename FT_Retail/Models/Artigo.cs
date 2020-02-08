using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class Artigo
    {

        [Display(Name = "PLU")]
        public int idArtigo { get; set; }
        [Display(Name = "Nome do Artigo")]
        public string Nome { get; set; }
        [Display(Name = "PVP")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double Preco { get; set; }

      
    }
}
