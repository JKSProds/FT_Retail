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
        public int IdArtigo { get; set; }
        [Display(Name = "Nome do Artigo")]
        public string NomeArtigo { get; set; }
        [Display(Name = "PVP")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double Preco { get; set; }
        public Promocao Promocao { get; set; }
        [Display(Name = "Nome da Secção")]
        public string NomeSeccao { get; set; }

        [Display(Name = "Dias de Validade")]
        public int DiasValidade { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Texto de Conservação")]
        public string TxtConservacao { get; set; }
        [Display(Name = "Texto de Informação de Utilização")]
        [DataType(DataType.MultilineText)]
        public string TxtInfoUtilizacao { get; set; }
        [Display(Name = "Ingredientes")]
        [DataType(DataType.MultilineText)]
        public string TxtIngredientes { get; set; }
        [Display(Name = "Texto de Informação Nutricional")]
        [DataType(DataType.MultilineText)]
        public string TxtInfoNutricional { get; set; }
        [Display(Name = "Texto de Alergénios")]
        [DataType(DataType.MultilineText)]
        public string TxtAlergenos { get; set; }
        [Display(Name = "Ultima Atualização")]
        public DateTime UltimaAtualizacao { get; set; }


    }
}
