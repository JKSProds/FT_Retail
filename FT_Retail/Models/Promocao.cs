using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class Promocao
    {
        public int IdArtigo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool PromocaoAtiva { get; set; }

        public Promocao(DateTime dI, DateTime dF, int iA)
        {
            IdArtigo = iA;
            DataInicio = dI;
            DataFim = dF;

            if (DataInicio >= DateTime.Now && DataFim <= DateTime.Now) {
                PromocaoAtiva = true;
            }
            else
            {
                PromocaoAtiva = false;
            }
        }
    }
}
