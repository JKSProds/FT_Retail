using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class Loja
    {
        [Display(Name = "ID da Loja")]
        public int IdLoja { get; set; }
        [Display(Name = "Nome da Empresa")]
        public string NomeEmpresa { get; set; }
        [Display(Name = "Nome da Loja")]
        public string NomeLoja { get; set; }
        [Display(Name = "Morada")]
        public string MoradaLoja { get; set; }
        [Display(Name = "Endereço IP do Computador")]
        public string IPLoja { get; set; }
        [Display(Name = "Nº de Balanças Ativas")]
        public int NBalancas { get; set; }
    }
}
