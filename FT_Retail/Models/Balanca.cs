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
        public int IDTipoBalanca { get; set; }
        public string TipoBalanca { get; set; }
        [Display(Name = "Envios Pendentes")]
        public string RegistoArtigo { get; set; }
        public string RegistoOferta { get; set; }
        [Display(Name = "Artigos Pendentes")]
        public int TransacoesPendentesArtigos { get; set; }
        [Display(Name = "Promoções Pendentes")]
        public int TransacoesPendentesOfertas { get; set; }
        public int TransacoesPendentes { get; set; }
        public int TransacoesErro { get; set; }
        public int EstadoEquipamento { get; set; }  // 1- Tudo OK 2 - Envios Pendentes 3 - Envios com Erro
        public string MensagemInfo { get; set; }
        [Display(Name = "Ultima Atualização com sucesso")]
        public DateTime UltimaAtualizacaoSucesso { get; set; }

        public void DefinirTipo()
        {

            switch (IDTipoBalanca)
            {
                case 0:
                    TipoBalanca = "CS1100";
                    break;
                case 1:
                    TipoBalanca = "L-8XX";
                    RegistoArtigo = "L2";
                    RegistoOferta = "OI";
                    break;
                case 20:
                    TipoBalanca = "Gama 500";
                    RegistoArtigo = "L2";
                    RegistoOferta = "OT";
                    break;
                case 40:
                    TipoBalanca = "D-900";
                    RegistoArtigo = "L2";
                    RegistoOferta = "OT";
                    break;
                case 60:
                    TipoBalanca = "LP3400";
                    RegistoArtigo = "L2";
                    RegistoOferta = "OI";
                    break;
            }
            DirecaoLogica = DirecaoLogica.ToString().PadLeft(2, '0');
        }

        public void DefinirEstado()
        {


            EstadoEquipamento = 1;
            if (TransacoesPendentes > 0) { EstadoEquipamento = 2; }
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
