using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class Balanca
    {
        public int IdBalanca { get; set; }
        public string NomeBalanca { get; set; }
        [Display(Name = "Endereço IP")]
        public string Dir_IP { get; set; }
        public string DirecaoLogica { get; set; }
        public int PortaTX { get; set; }
        public string TipoBalanca { get; set; }
        [Display(Name = "Envios Pendentes")]
        public int TransacoesPendentes { get; set; }
        [Display(Name = "Transações Enviadas")]
        public int TransacoesEnviadas { get; set; }
        public int TransacoesErro { get; set; }
        public int EstadoEquipamento { get; set; }  // 1- Tudo OK 2 - Envios Pendentes 3 - Envios com Erro
        public string MensagemInfo { get; set; }
        [Display(Name = "Utilma Atualização com sucesso")]
        public DateTime UltimaAtualizacaoSucesso { get; set; }

        public void DefinirEstado()
        {

            if (TipoBalanca == "20") { TipoBalanca = "Gama 500"; } else if (TipoBalanca == "40") { TipoBalanca = "D-900"; } else if (TipoBalanca == "60") { TipoBalanca = "LP-3400"; }
            DirecaoLogica = DirecaoLogica.ToString().PadLeft(2, '0');
            EstadoEquipamento = 1;
            if (TransacoesPendentes > 0 ) { EstadoEquipamento = 2; }
            if (TransacoesErro > 0) { EstadoEquipamento = 3; }

            switch (EstadoEquipamento)
            {
                case 1:
                    MensagemInfo = "A balança encontra-se atualizada, não existem envios pendentes, nem erros. Tudo OK";
                    break;
                case 2:
                    MensagemInfo = "Existem envios pendentes para a balança, mas sem erros. Aguarde mais alguns minutos e verifique novamente";
                    break;
                case 3:
                    MensagemInfo = "Existem envios com erro para esta balança. Por favor verifique se o cabo de rede se encontra corretamente ligado. Caso o problema se mantenha troque a balança de posição com outra balança deixando os cabos no sitio!";
                    break;
            }
        }
    }
}
