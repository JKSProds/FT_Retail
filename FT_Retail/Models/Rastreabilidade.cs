using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FT_Retail.Models
{
    public class Rastreabilidade
    {
        [Display(Name = "ID")]
        public int IDRast { get; set; }
        [Display(Name = "Nome")]
        public string NomeRastreabilidade { get; set; }
    }
}