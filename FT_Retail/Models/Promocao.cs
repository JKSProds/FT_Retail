using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class Promocao
    {
        public int IdArtigo { get; set; }
        [Display(Name = "PVP - Promoção")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double PrecoPromocao { get; set; }
        [Display(Name = "Data de Inicio da Promoção")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DataInicioPromocao { get; set; }
        [Display(Name = "Data de Fim da Promoção")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DataFimPromocao { get; set; }

        public bool PromocaoAtiva { get; set; }

        public Promocao(DateTime dI, DateTime dF, int iA, double pP)
        {
            IdArtigo = iA;
            DataInicioPromocao = dI;
            DataFimPromocao = dF;
            PrecoPromocao = pP;

            if (DataInicioPromocao <= DateTime.Now && DataFimPromocao >= DateTime.Now) {
                PromocaoAtiva = true;
            }
            else
            {
                PromocaoAtiva = false;
            }
        }
        public Promocao()
        {
            DataInicioPromocao = new DateTime();
            DataFimPromocao = new DateTime();
            PrecoPromocao = 0.00;
            PromocaoAtiva = false;
        }
    }
}
