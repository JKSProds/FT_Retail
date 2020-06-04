using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class ProcessedFile
    {
        public int IdFicheiro { get; set; }
        [Display(Name = "Nome")]
        public string NomeFicheiro { get; set; }
        [Display(Name = "Nº de Linhas")]
        public int NLinhas { get; set; }
        [Display(Name = "Data")]
        public DateTime DataModificacao { get; set; }
        [Display(Name = "Tamanho")]
        public string Size { get; set; }
        public string FullPath { get; set; }
    }
}
